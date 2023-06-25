using ETicaretAPI.Application.DTOs.AppUsers;

namespace ETicaretAPI.Application.Services.AppUser
{
    public interface IUserService
    {
        Task<CreateAppUserResponseDto> CreateAsync(CreateAppUserDto model);
    }
}
