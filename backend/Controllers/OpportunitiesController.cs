using AutoMapper;
using backend.Data;
using backend.DTOs;
using backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/opportunities")]
    [Authorize]
    [ApiController]
    public class OpportunitiesController : BaseApiController
    {
        private readonly CrmDBContext _context;
        private readonly IMapper _mapper;

        public OpportunitiesController(CrmDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<OpportunityDto>>> GetAllOpportunities([FromQuery] Guid? customerId)
        {
            var userId = GetUserId();

            var query =  _context.Opportunities
                .Where(c => c.UserId == userId)
                .Include(o => o.Customer)
                .AsQueryable();

            // 🔹 Filter by CustomerId if provided
            if (customerId.HasValue && customerId.Value != Guid.Empty)
            {
                query = query.Where(a => a.CustomerId == customerId.Value);
            }

            var opportunities = await query.ToListAsync();

            return Ok(_mapper.Map<List<OpportunityDto>>(opportunities));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OpportunityDto>> GetOpportunityById(Guid id)
        {
            var userId = GetUserId();

            var opportunity = await _context.Opportunities
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (opportunity == null)
                return NotFound(new { Message = "Opportunity not found" });

            return Ok(_mapper.Map<OpportunityDto>(opportunity));
        }

        [HttpPost]
        public async Task<ActionResult<OpportunityDto>> CreateOpportunity(CreateOpportunityDto dto)
        {
            var userId = GetUserId();

            var opportunity = _mapper.Map<Opportunity>(dto);
            opportunity.UserId = userId;

            _context.Opportunities.Add(opportunity);

            var saved = await _context.SaveChangesAsync() > 0;
            if (!saved) return BadRequest("Could not save changes to the database");

            return CreatedAtAction(nameof(GetOpportunityById),
                new { id = opportunity.Id }, _mapper.Map<OpportunityDto>(opportunity));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOpportunity(Guid id, UpdateOpportunityDto dto)
        {
            var userId = GetUserId();

            var opportunity = await _context.Opportunities.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (opportunity is null)
                return NotFound(new { Message = "Opportunity not found" });

            _mapper.Map(dto, opportunity);

            try
            {
                var saved = await _context.SaveChangesAsync() > 0;
                return Ok(new
                {
                    Message = saved ? "Opportunity updated successfully" : "No changes detected",
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
        public async Task<IActionResult> DeleteOpportunity(Guid id)
        {
            var userId = GetUserId();

            var opportunity = await _context.Opportunities.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (opportunity == null)
                return NotFound(new { Message = "Opportunity not found" });

            _context.Opportunities.Remove(opportunity);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Opportunity deleted successfully" });
        }
    }
}
