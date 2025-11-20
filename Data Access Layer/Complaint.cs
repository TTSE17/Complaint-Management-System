namespace Data_Access_Layer;

public class Complaint
{
    public int Id { get; init; }

    public int CitizenId { get; init; }

    public Citizen Citizen { get; init; } = null!;

    public int DepartmentId { get; init; }

    public Department Department { get; init; } = null!;

    public ComplaintStatus ComplaintStatus { get; init; }

    public string Title { get; init; }

    public string Description { get; set; } = null!;

    public string Location { get; set; } = null!;

    public DateTime StartDate { get; set; } = DateTime.Now;
}

public enum ComplaintStatus
{
    Pending,
    Resolved,
    Rejected,
}