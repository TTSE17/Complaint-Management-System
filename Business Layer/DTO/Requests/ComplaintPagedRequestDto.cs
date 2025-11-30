namespace Business_Layer.DTO.Requests;

public class ComplaintPagedRequestDto
{
    public string? Status { get; set; } = null;
    
    public int? CitizenId { get; set; } = null;
}