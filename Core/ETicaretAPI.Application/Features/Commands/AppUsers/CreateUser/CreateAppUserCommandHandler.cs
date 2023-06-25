using ETicaretAPI.Application.DTOs.AppUsers;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Services.AppUser;
using ETicaretAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUsers.CreateUser
{
    public class CreateAppUserCommandHandler : IRequestHandler<CreateAppUserCommandRequest, CreateAppUserCommandResponse>
    {
        readonly IUserService _userService;

        public CreateAppUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CreateAppUserCommandResponse> Handle(CreateAppUserCommandRequest request, CancellationToken cancellationToken)
        {
            CreateAppUserResponseDto responseDto= await _userService.CreateAsync(new()
            {
                Email = request.Email,
                Name = request.Name,
                Password = request.Password,
                PasswordConfirm = request.PasswordConfirm,
                UserName = request.UserName
            });
            return new()
            {
                Message = responseDto.Message,
                Result=responseDto.Result,
            };
                //throw new AppUserCreateFailedException();

        }
    }
}
