namespace Business_Layer.DTO.Requests;

public class LoginDto
{
    [Required(ErrorMessage = "Please Enter your Phone || Email")]

    public string EmailPhone { get; set; } = null!;

    [Required(ErrorMessage = "Please Enter your Password")]
    public string Password { get; set; } = null!;

    // [Required(ErrorMessage = "Please Enter your FCM")]
    public string? Fcm { get; set; } = null!;
}