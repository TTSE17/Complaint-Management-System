namespace Data_Access_Layer;

public class ComplaintHistory
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ComplaintId { get; set; }

    // public int CitizenId { get; set; }

    public int DepartmentId { get; set; }

    public Department Department { get; set; } = null!;

    public ComplaintStatus Status { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Location { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // public List<ComplaintHistory> ComplaintHistories { get; set; } = [];
}