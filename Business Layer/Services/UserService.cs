namespace Business_Layer.Services
{
    public class UserService(UserManager<User> userManager, IMapper mapper, AppDbContext context) : IUserService
    {
        public async Task<Response<User>> Register(CreateUserDto dto)
        {
            var response = new Response<User>();

            var firstName = dto.FirstName.Trim();
            var lastName = dto.LastName.Trim();
            var phoneNumber = dto.PhoneNumber.Trim();
            var email = dto.Email.Trim();
            var password = dto.Password.Trim();
            var userType = dto.UserType;

            try
            {
                var isNameExists =
                    await context.Users.AnyAsync(u => u.FirstName == firstName && u.LastName == lastName);

                if (isNameExists)
                {
                    response.Error = "Firstname and Lastname already exist, Please Login";

                    return response;
                }

                var isEmailExists = await context.Users.AnyAsync(u => u.Email == email);

                if (isEmailExists)
                {
                    response.Error = "Email Already Exists, Please Login";

                    return response;
                }

                var isPhoneExists = await context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);

                if (isPhoneExists)
                {
                    response.Error = "PhoneNumber Already Exists, Please Login";

                    return response;
                }

                var user = mapper.Map<User>(dto);

                user.UserType = userType;

                user.UserName = dto.Email;

                var result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                    response.Error = $"Password validation failed: {errors}";

                    return response;
                }

                response.Result = user;

                response.Success = true;
            }

            catch (Exception e)
            {
                response.Error = e.Message;
            }

            return response;
        }
    }
}