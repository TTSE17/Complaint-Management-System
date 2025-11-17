namespace Business_Layer.DTO.Responses;

public class AuthResponse
{
    public string UserId { get; set; } = null!;

    public GetUserDto User { get; set; } = null!;

    public string Token { get; set; } = null!;

    public string Type { get; set; } = null!;
}