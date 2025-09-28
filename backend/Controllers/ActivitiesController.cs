using AutoMapper;
using backend.Data;
using backend.DTOs;
using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/activities")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly CrmDBContext _context;
        private readonly IMapper _mapper;

        public ActivitiesController(CrmDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActivityDto>>> GetAllActivities()
        {
            var activities = await _context.Activities
            .Include(c => c.Customer)
            .ToListAsync();

            return _mapper.Map<List<ActivityDto>>(activities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDto>> GetActivityById(Guid id)
        {
            var activity = await _context.Activities
            .Include(c => c.Customer)
            .FirstOrDefaultAsync(x => x.Id == id);

            if (activity == null)
                return NotFound(new { Message = "Activity not found" });

            return _mapper.Map<ActivityDto>(activity);
        }

        [HttpPost]
        public async Task<ActionResult<ActivityDto>> CreateActivity(CreateActivityDto activityDto)
        {
            var activity = _mapper.Map<Activity>(activityDto);

            _context.Activities.Add(activity);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest("Could not save changes to the database");

            return CreatedAtAction(nameof(GetActivityById),
                new { activity.Id }, _mapper.Map<ActivityDto>(activity));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActivity(Guid id, UpdateActivityDto updateActivityDto)
        {
            var activity = await _context.Activities.FirstOrDefaultAsync(x => x.Id == id);
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
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
                return NotFound(new { Message = "Activity not found" });

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Activity deleted successfully" });
        }

    }
}
