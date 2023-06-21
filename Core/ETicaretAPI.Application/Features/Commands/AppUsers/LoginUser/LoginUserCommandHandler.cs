using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Services.TokenService;
using ETicaretAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUsers.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly ITokenHandler _tokenHandler;

        public LoginUserCommandHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            AppUser user = await _userManager.FindByNameAsync(request.UsernameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);
            if (user == null)
                throw new NotFoundUserException();

            if (!(user == null))
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (result.Succeeded)
                {
                    Token token = _tokenHandler.CreateAccessToken(15); 
                    return new LoginUserSuccessCommandResponse()
                    {
                        Token = token
                    };
                }
            }
            //return new LoginUserErrorCommandResponse()
            //{
            //    ErrorMessage = ""
            //};
            throw new AuthenticationErrorException();
        }
    }
}
