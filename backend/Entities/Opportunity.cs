using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

[Table("Opportunities")]

public class Opportunity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Value { get; set; }
    public string Stage { get; set; } = "Prospecting"; // e.g. Prospecting, Proposal, ClosedWon, ClosedLost

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
