namespace Business_Layer.DTO.Responses;

public class GetNotificationDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public GetUserDto? User { get; set; }

    public string Title { get; set; } = null!;

    public string? Text { get; set; }

    public DateTime CreatedDateTime { get; set; } = DateTime.Now;

    public DateTime? ReadAtDateTime { get; set; }

    public bool IsRead => ReadAtDateTime.HasValue;
}