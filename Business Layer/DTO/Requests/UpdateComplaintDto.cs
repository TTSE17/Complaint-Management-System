namespace Business_Layer.DTO.Requests;

public class UpdateComplaintDto
{
    public int Id { get; set; }

    public int DepartmentId { get; init; }

    public string Title { get; init; } = null!;

    public string Description { get; set; } = null!;

    public string Location { get; set; } = null!;

    public List<string> Paths { get; set; } = [];
}