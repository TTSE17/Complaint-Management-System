namespace Data_Access_Layer;

public class ComplaintHistory
{
    public int Id { get; set; }

    public int ComplaintId { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public string Comment { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}