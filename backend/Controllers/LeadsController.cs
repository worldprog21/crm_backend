using AutoMapper;
using backend.Data;
using backend.DTOs;
using backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/leads")]
    [Authorize]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        private readonly CrmDBContext _context;
        private readonly IMapper _mapper;

        public LeadsController(CrmDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<LeadDto>>> GetAllLeads()
        {
            var leads = await _context.Leads.ToListAsync();

            return _mapper.Map<List<LeadDto>>(leads);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeadDto>> GetLeadById(Guid id)
        {
            var lead = await _context.Leads.FirstOrDefaultAsync(x => x.Id == id);

            if (lead == null)
                return NotFound(new { Message = "Record not found" });

            return _mapper.Map<LeadDto>(lead);
        }

        [HttpPost]
        public async Task<ActionResult<LeadDto>> CreateLead(CreateLeadDto leadDto)
        {
            var lead = _mapper.Map<Lead>(leadDto);

            _context.Leads.Add(lead);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest(new { Message = "Could not save changes to the database" });

            return CreatedAtAction(nameof(GetLeadById),
                new { lead.Id }, _mapper.Map<LeadDto>(lead));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLead(Guid id, UpdateLeadDto updateLeadDto)
        {
            var lead = await _context.Leads.FirstOrDefaultAsync(x => x.Id == id);

            if (lead is null)
                return NotFound(new { Message = "Lead not found" });

            // Apply changes from DTO to entity
            _mapper.Map(updateLeadDto, lead);

            try
            {
                var saved = await _context.SaveChangesAsync() > 0;

                return Ok(new
                {
                    Message = saved ? "Lead updated successfully" : "No changes detected",
                    LeadId = lead.Id
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
        public async Task<IActionResult> DeleteLead(Guid id)
        {
            var lead = await _context.Leads.FindAsync(id);

            if (lead == null)
                return NotFound(new { Message = "Lead not found" });

            _context.Leads.Remove(lead);

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Lead deleted successfully" });
        }

    }
}
