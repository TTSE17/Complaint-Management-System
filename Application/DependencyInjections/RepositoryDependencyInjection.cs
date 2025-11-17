namespace Application.DependencyInjections
{
    public static class RepositoryDependencyInjection
    {
        public static IServiceCollection AddRepositoryDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IUserService, UserService>();
            
            services.AddScoped<TokenService>();

            return services;
        }
    }
}