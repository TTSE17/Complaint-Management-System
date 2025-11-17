using System.Reflection;

namespace Application.DependencyInjections;

public static class GeneralRegistrationDependencyInjection
{
    public static IServiceCollection AddGeneralDependencyInjection(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options => options
                    .UseSqlServer(connectionString, b => b.MigrationsAssembly(nameof(Application)))
                // .LogTo(Console.WriteLine, LogLevel.Information)
            )
            ; // Scoped

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        /*
         Sets up an in-memory distributed cache for storing session data.

         This is the simplest cache provider, storing data in the web server's memory.

         Note: Data will be lost if the app restarts.

         For production, you'd typically use a more persistent distributed cache like Redis or SQL Server.
         */
        services.AddDistributedMemoryCache();

        services.AddSession(opt =>
        {
            opt.IOTimeout = TimeSpan.FromMinutes(5);
            opt.IdleTimeout = TimeSpan.FromMinutes(5);
            opt.Cookie.Path = "/";
            opt.Cookie.IsEssential = true;
            opt.Cookie.HttpOnly = true;
            opt.Cookie.Name = "SimpleProject";
        });

        return services;
    }
}