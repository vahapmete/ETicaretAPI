using ETicaretAPI.Application.DTOs.AppUsers;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Application.Services.AppUser
{
    public interface IUserService
    {
        Task<CreateAppUserResponseDto> CreateAsync(CreateAppUserDto model);
      
    }
}
