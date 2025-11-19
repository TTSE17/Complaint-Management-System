namespace Business_Layer.DTO.Requests;

public class CreateUserDto
{
    [Required] [EmailAddress] public string Email { get; set; } = null!;
    [Required] [Phone] public string PhoneNumber { get; set; } = null!;
    public string Password { get; set; } = null!;
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string LastName { get; set; } = null!;
}