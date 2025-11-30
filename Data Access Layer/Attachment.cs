namespace Data_Access_Layer;

public class Attachment
{
    public int Id { get; set; }

    public int ComplaintId { get; set; }

    public Complaint Complaint { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public DateTime UploadedAt { get; set; } = DateTime.Now;
}