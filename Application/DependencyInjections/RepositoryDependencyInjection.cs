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

            services.AddScoped<IComplaintService, ComplaintService>();

            services.AddScoped<IDepartmentService, DepartmentService>();

            services.AddScoped<IEmployeeService, EmployeeService>();

            return services;
        }
    }
}