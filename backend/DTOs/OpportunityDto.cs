using System;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs;


public class OpportunityDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal Value { get; set; }
    public string Stage { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateOpportunityDto
{
    [Required]
    public string Title { get; set; }
    [Required]
    public decimal Value { get; set; }
    [Required]
    public string Stage { get; set; }
    [Required]
    public Guid CustomerId { get; set; }

}

public class UpdateOpportunityDto
{
    public string? Title { get; set; }
    public decimal? Value { get; set; }
    public string? Stage { get; set; }
    public Guid? CustomerId { get; set; }
}

