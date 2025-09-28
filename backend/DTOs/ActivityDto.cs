using System;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs;


public class ActivityDto
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Notes { get; set; }
    public DateTime DueDate { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateActivityDto
{
    [Required]
    public string Type { get; set; }
    [Required]
    public string Notes { get; set; }
    [Required]
    public DateTime DueDate { get; set; }
    [Required]
    public string Phone { get; set; }
    [Required]
    public Guid CustomerId { get; set; }

}

public class UpdateActivityDto
{
    public string? Type { get; set; }
    public string? Notes { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Phone { get; set; }
    public Guid? CustomerId { get; set; }
}

