namespace Application.DependencyInjections
{
    public static class RepositoryDependencyInjection
    {
        public static IServiceCollection AddRepositoryDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IClientService, ClientService>();

            services.AddScoped<TokenService>();

            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}