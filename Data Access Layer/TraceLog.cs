namespace Data_Access_Layer;

public class TraceLog
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string? UserName { get; set; }
    public string Action { get; set; } = null!;
    public bool IsSuccess { get; set; }
    public string? Exception { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
