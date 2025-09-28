using AutoMapper;
using backend.Data;
using backend.DTOs;
using backend.Entities;
using backend.RequestHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CrmDBContext _context;
        private readonly IMapper _mapper;

        public CustomersController(CrmDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerDto>>> GetAllCustomers([FromQuery] PaginationParams paginationParams)
        {
            var query = _context.Customers
            .Include(c => c.Contacts) // load contact
            .Include(c => c.Opportunities) // load Opportunities
            .AsQueryable();

            var totalCount = await query.CountAsync();

            var customers = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            var customerDtos = _mapper.Map<List<CustomerDto>>(customers);

            return Ok(new
            {
                TotalCount = totalCount,
                PageNumber = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationParams.PageSize),
                Data = customerDtos
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(Guid id)
        {
            var customer = await _context.Customers
            .Include(c => c.Contacts) // load contact
            .Include(c => c.Opportunities) // load Opportunities
            .FirstOrDefaultAsync(x => x.Id == id);

            if (customer == null)
                return NotFound(new { Message = "Customer not found" });

            return _mapper.Map<CustomerDto>(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto CustomerDto)
        {
            var customer = _mapper.Map<Customer>(CustomerDto);

            _context.Customers.Add(customer);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest("Could not save changes to the database");

            return CreatedAtAction(nameof(GetCustomerById),
                new { customer.Id }, _mapper.Map<CustomerDto>(customer));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid id, UpdateCustomerDto updateCustomerDto)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (customer is null)
                return NotFound(new { Message = "Customer not found" });

            // Apply changes from DTO to entity
            _mapper.Map(updateCustomerDto, customer);

            try
            {
                var saved = await _context.SaveChangesAsync() > 0;
                return Ok(new
                {
                    Message = saved ? "Customer updated successfully" : "No changes detected",
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
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound(new { Message = "Customer not found" });

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Customer deleted successfully" });
        }

    }
}
