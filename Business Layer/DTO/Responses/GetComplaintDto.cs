namespace Business_Layer.DTO.Responses;

public class GetComplaintDto
{
    public int Id { get; init; }

    public int CitizenId { get; init; }
    public string CitizenName { get; init; } = null!;

    public int DepartmentId { get; init; }
    public string DepartmentName { get; init; } = null!;

    public string Status { get; init; } = null!;

    public string Title { get; init; } = null!;

    public string Description { get; set; } = null!;

    public string Location { get; set; } = null!;

    public DateTime StartDate { get; set; } = DateTime.Now;

    public List<string>? Attachments { get; set; } = [];
}