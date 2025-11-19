using Microsoft.AspNetCore.Identity;

namespace Application.DependencyInjections;

public static class IdentityDependencyInjection
{
    public static IServiceCollection AddIdentityDependencyInjection(this IServiceCollection services)
    {
        services
            .AddIdentity<User /*IdentityUser*/, IdentityRole<int>>
            (
                opt =>
                {
                    opt.Password.RequireDigit = false;
                    opt.Password.RequireLowercase = false;
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireNonAlphanumeric = false;
                    // opt.Password.RequiredUniqueChars = 5;
                    opt.Password.RequiredLength = 7;

                    opt.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";

                    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    opt.Lockout.MaxFailedAccessAttempts = 5;
                    opt.Lockout.AllowedForNewUsers = true;

                    opt.User.RequireUniqueEmail = false;

                    opt.SignIn.RequireConfirmedEmail = false;
                }
            )
            // This method registers UserStore and RoleStore, allowing Identity to interact with the database for user authentication.
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("CreateProduct",
                policy => policy.RequireAssertion(
                    context => context.User.IsInRole("Admin") &&
                               context.User.HasClaim("Create Product", "True")
                ));

            opt.AddPolicy("CreateRole",
                policy => policy.RequireClaim("Create Role"));
        });


        return services;
    }
}