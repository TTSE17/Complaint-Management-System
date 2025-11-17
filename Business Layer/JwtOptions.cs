namespace Business_Layer;

public class JwtOptions
{
    public string SigningKey { get; set; } = null!;

    public string Issuer { get; set; } = null!;

    public string Audience { get; set; } = null!;

    public int LifeTime { get; set; }
}