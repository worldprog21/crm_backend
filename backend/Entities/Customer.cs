using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

[Table("Customers")]

public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Industry { get; set; }
    public string Address { get; set; }
    public string Type { get; set; } = "Individual"; // Individual, Organization
    public ICollection<Contact>? Contacts { get; set; } = new List<Contact>();
    public ICollection<Opportunity>? Opportunities { get; set; } = new List<Opportunity>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
