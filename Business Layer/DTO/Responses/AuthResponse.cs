namespace Business_Layer.DTO.Responses;

public class AuthResponse
{
    public int UserId { get; set; }

    public GetUserDto User { get; set; } = null!;

    public string Token { get; set; } = null!;

    public string Type { get; set; } = null!;

    public bool IsEmailConfirmed { get; set; }
}