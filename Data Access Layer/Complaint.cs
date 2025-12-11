namespace Data_Access_Layer;

public class Complaint
{
    public int Id { get; set; }

    public int CitizenId { get; set; }

    public Citizen Citizen { get; set; } = null!;

    public int DepartmentId { get; set; }

    public Department Department { get; set; } = null!;

    public ComplaintStatus Status { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Location { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string HistoryJson { get; set; } = ""; // array of previous versions    

    public List<Attachment> Attachments { get; set; } = [];

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

}

public enum ComplaintStatus
{
    Pending,
    Resolved,
    Rejected,
}