namespace Data_Access_Layer;

public class Employee
{
    [Key] public int UserId { get; set; }

    [ForeignKey("UserId")] public User User { get; set; } = null!;

    public int DepartmentId { get; set; }

    public Department Department { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}