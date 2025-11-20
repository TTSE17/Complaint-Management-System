using Business_Layer.Consts;

namespace Application.Seeds;

public static class DefaultUsers
{
    public static async Task SeedBasicUserAsync(UserManager<User> userManager)
    {
        var defaultUser = new User
        {
            UserName = "employee@domain.com",
            Email = "employee@domain.com",
            EmailConfirmed = true
        };

        var user = await userManager.FindByEmailAsync(defaultUser.Email);

        if (user == null)
        {
            await userManager.CreateAsync(defaultUser, "a123456");

            await userManager.AddToRoleAsync(defaultUser, Roles.Employee);
        }
    }

    public static async Task SeedAdminUserAsync(UserManager<User> userManager,
        RoleManager<IdentityRole<int>> roleManger)
    {
        var defaultAdmin = new User
        {
            FirstName = "admin",
            LastName = "admin",
            UserName = "admin",
            UserType = UserType.Admiin,
            Email = "admin@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "0987654321",
            PhoneNumberConfirmed = true
        };

        var user = await userManager.FindByEmailAsync(defaultAdmin.Email);

        if (user == null)
        {
            await userManager.CreateAsync(defaultAdmin, "1234567");

            await userManager.AddToRoleAsync(defaultAdmin, Roles.Admin);
        }
    }
}