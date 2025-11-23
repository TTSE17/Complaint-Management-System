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
                var isNameExists = await IsNameExists(firstName, lastName);

                if (isNameExists)
                {
                    response.Error = "Firstname and Lastname already exist, Please Login";

                    return response;
                }

                var isEmailExists = await IsEmailExists(email);

                if (isEmailExists)
                {
                    response.Error = "Email Already Exists, Please Login";

                    return response;
                }

                var isPhoneExists = await IsPhoneNumberExists(phoneNumber);

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

        public async Task<Response<GetUserDto>> Update(UpdateUserDto dto)
        {
            var response = new Response<GetUserDto>();

            try
            {
                var id = dto.Id;
                var firstName = dto.FirstName?.Trim();
                var lastName = dto.LastName?.Trim();
                var phoneNumber = dto.PhoneNumber?.Trim();
                var password = dto.Password?.Trim();
                var oldPassword = dto.OldPassword?.Trim();

                var user = await userManager.FindByIdAsync(id.ToString());

                if (user == null)
                {
                    response.Error = "User Not Found";

                    return response;
                }

                // var updatedUser = mapper.Map(dto, user);

                if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(oldPassword))
                {
                    // await userManager.RemovePasswordAsync(user);
                    // user.PasswordHash = userManager.PasswordHasher.HashPassword(user, dto.Password);

                    var result = await userManager.ChangePasswordAsync(user, oldPassword, password);

                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                        response.Error = $"{errors}";

                        return response;
                    }
                }

                if (firstName != null || lastName != null)
                {
                    var fr = firstName ?? user.FirstName;

                    var ln = lastName ?? user.LastName;

                    var isNameExists = await IsNameExists(fr, ln, id);

                    if (isNameExists)
                    {
                        response.Error = "Firstname and Lastname already exist, Please Login";

                        return response;
                    }

                    if (firstName != null)
                    {
                        user.FirstName = firstName;
                    }

                    if (lastName != null)
                    {
                        user.LastName = lastName;
                    }
                }

                if (phoneNumber != null)
                {
                    var isPhoneExists = await IsPhoneNumberExists(phoneNumber, id);

                    if (isPhoneExists)
                    {
                        response.Error = "PhoneNumber Already Exists, Please Login";

                        return response;
                    }

                    user.PhoneNumber = phoneNumber;
                }

                await userManager.UpdateAsync(user);

                response.Result = mapper.Map<GetUserDto>(user);
            }
            catch (Exception e)
            {
                response.Error = e.Message + "\n";

                return response;
            }

            response.Success = true;

            return response;
        }

        private async Task<bool> IsNameExists(string firstName, string lastName, int? userId = null)
        {
            return await context.Users.AnyAsync(u =>
                u.FirstName == firstName && u.LastName == lastName && (userId == null || u.Id != userId));
        }

        private async Task<bool> IsPhoneNumberExists(string phoneNumber, int? userId = null)
        {
            return await context.Users.AnyAsync(u =>
                u.PhoneNumber == phoneNumber && (userId == null || u.Id != userId));
        }

        private async Task<bool> IsEmailExists(string email, int? userId = null)
        {
            return await context.Users.AnyAsync(u => u.Email == email && (userId == null || u.Id != userId));
        }
    }
}