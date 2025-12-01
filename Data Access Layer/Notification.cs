namespace Data_Access_Layer;

public class Notification
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Text { get; set; }

    public DateTime CreatedDateTime { get; set; } = DateTime.Now;

    public DateTime? ReadAtDateTime { get; set; }

    public bool IsRead => ReadAtDateTime.HasValue;
}