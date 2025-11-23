namespace Business_Layer.DTO.Requests;

public class UpdateUserDto
{
    public int Id { get; set; }

    [EmailAddress] public string? Email { get; set; } = null!;

    [Phone] public string? PhoneNumber { get; set; } = null!;
    public string? Password { get; set; } = null!;

    public string? FirstName { get; set; } = null!;

    public string? LastName { get; set; } = null!;

    public string? OldPassword { get; set; } = null!;
}