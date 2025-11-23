namespace Business_Layer.Services;

public class AccountService(
    ICitizenService citizenService,
    IAdminService adminService,
    IEmployeeService employeeService,
    SignInManager<User> signInManager,
    AppDbContext context) : IAccountService
{
    public async Task<Response<AuthResponse>> Login(LoginDto dto)
    {
        var response = new Response<AuthResponse>();

        var checkLogin = await CheckLogin(dto);

        if (!checkLogin.Success)
        {
            response.Error = checkLogin.Error;

            return response;
        }

        var user = checkLogin.Result;

        return user.UserType switch
        {
            UserType.Citizen => await citizenService.Login(user),

            UserType.Employee => await employeeService.Login(user),

            UserType.Admin => await adminService.Login(user),

            _ => new Response<AuthResponse> { Error = "Unknown user type" }
        };
    }

    private async Task<Response<User>> CheckLogin(LoginDto dto)
    {
        var response = new Response<User>();

        // var isValidEmail = new EmailAddressAttribute().IsValid(dto.UserNameEmail);
        //
        // User? user;
        //
        // if (isValidEmail)
        // {
        //     user = await userManager.FindByEmailAsync(dto.UserNameEmail);
        // }
        //
        // else
        // {
        //     user = await userManager.FindByNameAsync(dto.UserNameEmail);
        // }

        var emailPhone = dto.EmailPhone.Trim();

        var user = await context.Users
            .Where(u => u.Email == emailPhone || u.PhoneNumber == emailPhone)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            response.Error = "Unable to Log In";

            return response;
        }

        // var validPassword = await userManager.CheckPasswordAsync(user, dto.Password);
        //
        // if (validPassword == false)
        // {
        //     response.Error = "Unable to Log In";
        //
        //     return response;
        // }

        var result = await signInManager.PasswordSignInAsync(user, dto.Password, false, false);

        if (!result.Succeeded)
        {
            response.Error = "Unable to Log In";

            return response;
        }

        if (dto.Fcm != null)
        {
            user.Fcm = dto.Fcm;

            await context.SaveChangesAsync();
        }

        response.Result = user;

        response.Success = true;

        return response;
    }

    public async Task Logout()
    {
        await signInManager.SignOutAsync();
    }
}