using ETicaretAPI.Application.DTOs.AppUsers;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Services.AppUser;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;


namespace ETicaretAPI.Persistence.Sevices.AppUsers
{
    public class UserService : IUserService
    {
        readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateAppUserResponseDto> CreateAsync(CreateAppUserDto model)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Name = model.Name,
                Email = model.Email,
            }, model.Password);

            CreateAppUserResponseDto  response = new() { Result = result.Succeeded };


            if (result.Succeeded)
            {
                response.Message = "User created successfuly!";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    response.Message += $"{error.Code} - {error.Description}\n";
                }
            }
            return response;
        }

        
    }
}
