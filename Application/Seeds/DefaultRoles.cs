using Business_Layer.Consts;
using Microsoft.AspNetCore.Identity;

namespace Application.Seeds;

public static class DefaultRoles
{
    public static async Task SeedAsync(RoleManager<IdentityRole<int>> roleManger)
    {
        if (!roleManger.Roles.Any(r => r.Name == Roles.Admin))
            await roleManger.CreateAsync(new IdentityRole<int>(Roles.Admin));

        if (!roleManger.Roles.Any(r => r.Name == Roles.Citizen))
            await roleManger.CreateAsync(new IdentityRole<int>(Roles.Citizen));

        if (!roleManger.Roles.Any(r => r.Name == Roles.Employee))
            await roleManger.CreateAsync(new IdentityRole<int>(Roles.Employee));
    }
}