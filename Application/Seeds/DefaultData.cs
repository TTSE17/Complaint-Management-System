namespace Application.Seeds;

public static class DefaultData
{
    public static async Task SeedData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;

        var loggerFactory = services.GetRequiredService<ILoggerProvider>();

        var logger = loggerFactory.CreateLogger("app");

        try
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

            var userManager = services.GetRequiredService<UserManager<User>>();

            await DefaultRoles.SeedAsync(roleManager);

            await DefaultUsers.SeedAdminUserAsync(userManager, roleManager);

            logger.LogInformation("Data seeded");

            logger.LogInformation($"Application Started at : {DateTime.Now}");
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "An error occurred while seeding data");
        }
    }
}