namespace Business_Layer.DTO.Responses;

public class ComplaintSnapshot
{
    public int Version { get; set; }
    public int UserId { get; set; }
    public int DepartmentId { get; set; }
    public ComplaintStatus Status { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string Action { get; set; } = null!;
    public DateTime ChangedAt { get; set; }
    public List<string> Attachments { get; set; } = [];
}