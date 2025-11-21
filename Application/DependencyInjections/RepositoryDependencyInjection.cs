namespace Application.DependencyInjections
{
    public static class RepositoryDependencyInjection
    {
        public static IServiceCollection AddRepositoryDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IUserService, UserService>();

            services.AddTransient<ICitizenService, CitizenService>();

            services.AddScoped<TokenService>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddTransient<IComplaintService, ComplaintService>();

            return services;
        }
    }
}