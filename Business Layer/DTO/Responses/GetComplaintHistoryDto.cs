namespace Business_Layer.DTO.Responses;

public class GetComplaintHistoryDto
{
    public string Comment { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}