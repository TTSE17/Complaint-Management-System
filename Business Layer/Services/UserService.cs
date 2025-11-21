namespace Business_Layer.Services
{
    public class UserService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IMapper mapper,
        AppDbContext context,
        TokenService tokenService) : IUserService
    {
        public async Task<Response<AuthResponse>> Login(LoginDto dto)
        {
            var response = new Response<AuthResponse>();

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

            if (user == null || user.UserType == UserType.Citizen)
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

            response.Success = true;

            response.Result = new AuthResponse
            {
                Token = await tokenService.CreateJwtToken(user),
                UserId = user.Id,
                User = mapper.Map<GetUserDto>(user),
                Type = user.UserType.ToString()
            };

            return response;
        }

        // public async Task<Response<GetUserDto>> Update(UpdateUserDto dto)
        // {
        //     var response = new Response<GetUserDto>();
        //
        //     try
        //     {
        //         var user = await userManager.FindByIdAsync(dto.Id);
        //
        //         if (user == null)
        //         {
        //             response.Error = "User Not Found";
        //
        //             return response;
        //         }
        //
        //         var updatedUser = mapper.Map(dto, user);
        //
        //         if (!string.IsNullOrEmpty(dto.Password) && !string.IsNullOrEmpty(dto.OldPassword))
        //         {
        //             // await userManager.RemovePasswordAsync(user);
        //             // user.PasswordHash = userManager.PasswordHasher.HashPassword(user, dto.Password);
        //
        //             var result = await userManager.ChangePasswordAsync(user, dto.OldPassword, dto.Password);
        //
        //             if (!result.Succeeded)
        //             {
        //                 var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        //                 response.Error = $"Password change failed: {errors}";
        //                 return response;
        //             }
        //         }
        //
        //         await userManager.UpdateNormalizedUserNameAsync(updatedUser);
        //
        //         await userManager.UpdateNormalizedEmailAsync(updatedUser);
        //
        //         await userManager.UpdateAsync(updatedUser);
        //
        //         response.Result = mapper.Map<GetUserDto>(updatedUser);
        //     }
        //     catch (Exception e)
        //     {
        //         response.Error = e.Message + "\n";
        //     }
        //
        //     response.Success = true;
        //
        //     return response;
        // }
        
        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }
    }
}