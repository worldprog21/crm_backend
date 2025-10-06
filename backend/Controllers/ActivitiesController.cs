using AutoMapper;
using backend.Data;
using backend.DTOs;
using backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/activities")]
    [Authorize]
    [ApiController]
    public class ActivitiesController : BaseApiController
    {
        private readonly CrmDBContext _context;
        private readonly IMapper _mapper;

        public ActivitiesController(CrmDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActivityDto>>> GetAllActivities([FromQuery] Guid? customerId)
        {
            var userId = GetUserId();

            var query = _context.Activities
            .Where(c => c.UserId == userId)
            .Include(c => c.Customer)
            .AsQueryable();

             // 🔹 Filter by CustomerId if provided
            if (customerId.HasValue && customerId.Value != Guid.Empty)
            {
                query = query.Where(a => a.CustomerId == customerId.Value);
            }

            var activities = await query.ToListAsync();

            return _mapper.Map<List<ActivityDto>>(activities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDto>> GetActivityById(Guid id)
        {
            var userId = GetUserId();

            var activity = await _context.Activities
            .Include(c => c.Customer)
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (activity == null)
                return NotFound(new { Message = "Activity not found" });

            return _mapper.Map<ActivityDto>(activity);
        }

        [HttpPost]
        public async Task<ActionResult<ActivityDto>> CreateActivity(CreateActivityDto activityDto)
        {
            var userId = GetUserId();

            var activity = _mapper.Map<Activity>(activityDto);
            activity.UserId = userId;

            _context.Activities.Add(activity);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest("Could not save changes to the database");

            return CreatedAtAction(nameof(GetActivityById),
                new { activity.Id }, _mapper.Map<ActivityDto>(activity));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActivity(Guid id, UpdateActivityDto updateActivityDto)
        {
            var userId = GetUserId();
            
            var activity = await _context.Activities.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (activity is null)
                return NotFound(new { Message = "Activity not found" });

            // Apply changes from DTO to entity
            _mapper.Map(updateActivityDto, activity);

            try
            {
                var saved = await _context.SaveChangesAsync() > 0;
                return Ok(new
                {
                    Message = saved ? "Activity updated successfully" : "No changes detected",
                });
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new
                {
                    Message = "Database update error",
                    Details = dbEx.InnerException?.Message ?? dbEx.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            var userId = GetUserId();
            
            var activity = await _context.Activities.FirstOrDefaultAsync(x => x.Id == id&& x.UserId == userId);
            if (activity == null)
                return NotFound(new { Message = "Activity not found" });

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Activity deleted successfully" });
        }

    }
}
