
using Microsoft.EntityFrameworkCore;

namespace Business_Layer.Services;

public class AdminService(
    AppDbContext context,
    IMapper mapper,
    TokenService tokenService
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


}
