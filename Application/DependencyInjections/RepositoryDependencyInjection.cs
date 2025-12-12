using Business_Layer.Helpers;

namespace Application.DependencyInjections
{
    public static class RepositoryDependencyInjection
    {
        public static IServiceCollection AddRepositoryDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ICitizenService, CitizenService>();

            services.AddScoped<TokenService>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IComplaintService, ComplaintService>();

            services.AddScoped<IDepartmentService, DepartmentService>();

            services.AddScoped<IEmployeeService, EmployeeService>();

            services.AddScoped<IAdminService, AdminService>();

            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IFirebaseService, FirebaseService>();

            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<ITraceLogService, TraceLogService>();

            services.AddScoped<LoggerHelper>();


            return services;
        }
    }
}