using System.ComponentModel.DataAnnotations;
using backend.Entities;

namespace backend.DTOs;

public class CustomerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Industry { get; set; }
    public string Address { get; set; }
    public string Type { get; set; }
    public List<ContactDto> Contacts { get; set; } = new List<ContactDto>();
    public List<OpportunityDto> Opportunities { get; set; } = new List<OpportunityDto>();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateCustomerDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Industry { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string Type { get; set; }
}

public class UpdateCustomerDto
{
    public string? Name { get; set; }
    public string? Industry { get; set; }
    public string? Address { get; set; }
    public string? Type { get; set; }
}
