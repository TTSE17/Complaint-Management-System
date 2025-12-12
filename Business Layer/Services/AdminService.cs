
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business_Layer.Services;

public class AdminService(
    AppDbContext context,
    IMapper mapper,
    TokenService tokenService,
    UserManager<User> userManager
    ) : IAdminService
{
    //public async Task<Response<AuthResponse>> Login(User user)
    //{
    //    var response = new Response<AuthResponse>();

    //    var token = await tokenService.CreateJwtToken(user);

    //    var userDto = mapper.Map<GetUserDto>(user);

    //    response.Result = new AuthResponse
    //    {
    //        Token = token,
    //        UserId = user.Id,
    //        User = userDto,
    //        Type = user.UserType.ToString(),
    //        IsEmailConfirmed = true
    //    };

    //    response.Success = true;

    //    return response;
    //}

    public async Task<Response<AuthResponse>> Login(User user)
    {
        var response = new Response<AuthResponse>();

        var token = await tokenService.CreateJwtToken(user);

        var userDto = mapper.Map<GetUserDto>(user);

        response.Result = new AuthResponse
        {
            Token = token,
            UserId = user.Id,
            User = userDto,
            Type = user.UserType.ToString(),
            IsEmailConfirmed = true
        };

        response.Success = true;

        return response;
    }
    public async Task<Response<int>> GetUsersCount()
    {
        var response = new Response<int>();

        var count = await context.Users.CountAsync();

        response.Result = count;
        response.Success = true;

        return response;
    }

    public async Task<Response<int>> GetComplaintsCount()
    {
        var response = new Response<int>();

        var count = await context.Complaints.CountAsync();

        response.Result = count;
        response.Success = true;

        return response;
    }

    public async Task<Response<int>> GetComplaintsRejectedCount()
    {
        var response = new Response<int>();

        var count = await context.Complaints
            .Where(c => c.Status == ComplaintStatus.Rejected)
            .CountAsync();

        response.Result = count;
        response.Success = true;

        return response;
    }

    public async Task<Response<int>> GetComplaintsResolvedCount()
    {
        var response = new Response<int>();

        var count = await context.Complaints
            .Where(c => c.Status == ComplaintStatus.Resolved)
            .CountAsync();

        response.Result = count;
        response.Success = true;

        return response;
    }

    public async Task<Response<int>> GetComplaintsPendingCount()
    {
        var response = new Response<int>();

        var count = await context.Complaints
            .Where(c => c.Status == ComplaintStatus.Pending)
            .CountAsync();

        response.Result = count;
        response.Success = true;

        return response;
    }

    public async Task<Response<List<WeeklyDataDto>>> GetWeeklyCompletedStats()
    {
        var response = new Response<List<WeeklyDataDto>>();

        // الأيام بالعربي
        var days = new Dictionary<DayOfWeek, string>
    {
        { DayOfWeek.Saturday, "السبت" },
        { DayOfWeek.Sunday, "الأحد" },
        { DayOfWeek.Monday, "الاثنين" },
        { DayOfWeek.Tuesday, "الثلاثاء" },
        { DayOfWeek.Wednesday, "الأربعاء" },
        { DayOfWeek.Thursday, "الخميس" },
        { DayOfWeek.Friday, "الجمعة" }
    };

        // اجلب الشكاوى المنجزة في آخر 7 أيام
        var weekAgo = DateTime.Now.AddDays(-7);

        var complaints = await context.Complaints
            .Where(c => c.Status == ComplaintStatus.Resolved && c.UpdatedAt >= weekAgo)
            .ToListAsync();

        var result = days.Select(d => new WeeklyDataDto
        {
            Day = d.Value,
            Value = complaints.Count(c => c.UpdatedAt.DayOfWeek == d.Key)
        }).ToList();

        response.Result = result;
        response.Success = true;

        return response;
    }
    public async Task<Response<EmployeeCreatedResponse>> CreateEmployee(CreateEmployeeDto dto)
    {
        var response = new Response<EmployeeCreatedResponse>();

        // إنشاء كلمة سر عشوائية
        var password = Guid.NewGuid().ToString("N")[..8];

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserName = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            UserType = UserType.Employee,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            response.Error = string.Join(" | ", result.Errors.Select(e => e.Description));
            return response;
        }

        // إضافة Role
        await userManager.AddToRoleAsync(user, "Employee");

        var employee = new Employee
        {
            UserId = user.Id,
            DepartmentId = dto.DepartmentId,
        };

        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        


        response.Result = new EmployeeCreatedResponse
        {
            UserId = user.Id,
            FullName = $"{user.FirstName} {user.LastName}",
            Email = user.Email,
            Password = password
        };

        response.Success = true;
        return response;
    }

    public async Task<Response<List<AdminGetUserDto>>> GetAllUsers()
    {
        var response = new Response<List<AdminGetUserDto>>();

        var users = await context.Users.ToListAsync();

        response.Result = users.Select(u => new AdminGetUserDto
        {
            Id = u.Id,
            FullName = $"{u.FirstName} {u.LastName}",
            Email = u.Email!,
            PhoneNumber = u.PhoneNumber!,
            UserType = u.UserType.ToString()
        }).ToList();

        response.Success = true;

        return response;
    }




}
