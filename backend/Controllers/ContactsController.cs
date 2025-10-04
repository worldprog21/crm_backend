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
    [Route("api/contacts")]
    [ApiController]
    [Authorize]

    public class ContactsController : ControllerBase
    {
        private readonly CrmDBContext _context;
        private readonly IMapper _mapper;

        public ContactsController(CrmDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ContactDto>>> GetAllContacts()
        {
            var contacts = await _context.Contacts
            .Include(c => c.Customer)
            .ToListAsync();

            return _mapper.Map<List<ContactDto>>(contacts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDto>> GetContactById(Guid id)
        {
            var contact = await _context.Contacts
            .Include(c => c.Customer)
            .FirstOrDefaultAsync(x => x.Id == id);

            if (contact == null)
                return NotFound(new { Message = "Contact not found" });

            return _mapper.Map<ContactDto>(contact);
        }

        [HttpPost]
        public async Task<ActionResult<ContactDto>> CreateContact(CreateContactDto ContactDto)
        {
            var contact = _mapper.Map<Contact>(ContactDto);

            _context.Contacts.Add(contact);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest("Could not save changes to the database");

            return CreatedAtAction(nameof(GetContactById),
                new { contact.Id }, _mapper.Map<ContactDto>(contact));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(Guid id, UpdateContactDto updateContactDto)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == id);
            if (contact is null)
                return NotFound(new { Message = "Contact not found" });

            // Apply changes from DTO to entity
            _mapper.Map(updateContactDto, contact);

            try
            {
                var saved = await _context.SaveChangesAsync() > 0;
                return Ok(new
                {
                    Message = saved ? "Contact updated successfully" : "No changes detected",
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
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
                return NotFound(new { Message = "Contact not found" });

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Contact deleted successfully" });
        }

    }
}
