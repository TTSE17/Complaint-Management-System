namespace Business_Layer.DTO.Responses;

public class GetComplaintSnapshot : ComplaintSnapshot
{
    public string UserName { get; set; } = null!;
    public string DepartmentName { get; set; } = null!;

    public new string Status { get; set; } = null!;
}