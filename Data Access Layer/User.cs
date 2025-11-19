namespace Data_Access_Layer;

public class User : IdentityUser<int>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
    public int Type { get; set; }
    public string? Fcm { get; set; }


    [NotMapped]
    public UserType UserType
    {
        get => (UserType)Type;
        set => Type = (int)value;
    }
}