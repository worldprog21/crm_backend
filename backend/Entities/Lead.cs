using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

[Table("Leads")]

public class Lead
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; } = null;

    public string Status { get; set; } = "New"; // New, Contacted, Qualified, Disqualified
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

}
