using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

[Table("Activities")]

public class Activity
{
    public Guid Id { get; set; }
    public string Type { get; set; } = "Call"; // Call, Email, Meeting, Task
    public string Notes { get; set; }
    public DateTime DueDate { get; set; }
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

}
