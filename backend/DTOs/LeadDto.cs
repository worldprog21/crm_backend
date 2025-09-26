namespace backend.DTOs;

public class LeadBaseDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Status { get; set; }

}

public class LeadDto : LeadBaseDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class CreateLeadDto : LeadBaseDto {}
public class UpdateLeadDto : LeadBaseDto {}
