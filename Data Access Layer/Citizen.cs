namespace Data_Access_Layer;

public class Citizen
{
    [Key] public int UserId { get; set; }

    [ForeignKey("UserId")] public User User { get; set; } = null!;

    public string? OTP { get; set; }

    public DateTime? OTPExpirationTime { get; set; }
}