using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Features.Commands.AppUsers.LoginUser;
using ETicaretAPI.Application.Services.AppUser;
using ETicaretAPI.Application.Services.TokenService;
using ETicaretAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services.AppUsers
{
    public class AuthService : IAuthService
    {
        readonly IConfiguration _configuration;
        readonly ITokenHandler _tokenHandler;
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly IUserService _userService;


        public AuthService(IConfiguration configuration, UserManager<AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager, IUserService userService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
            _userService = userService;
        }

        public async Task<Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime) 
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["ExternalLoginSettings:Google:Client_ID"] }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");
            AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            bool result = user != null;
            if (user == null)
            {

                user = await _userManager.FindByEmailAsync(payload.Email);// ayni emailde iki hesap varsa patlar.

                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = payload.Email,
                        Name = payload.Name,
                        UserName = payload.Email
                    };
                    var identiyResult = await _userManager.CreateAsync(user);
                    result = identiyResult.Succeeded;
                }

            }
            if (result)
                await _userManager.AddLoginAsync(user, info);
            else
                throw new Exception("Invalid external authentication");

             Token token = _tokenHandler.CreateAccessToken(15);
             await  _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 5);
             return token;
            
        }

        public async Task<Token> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime)
        {
            // burada ayni mail ile iki farkli kaydin olmasinin onune gecmiyoruz. buda google login kisminda kullanici sorgusu yapildiginda  hata veriyor.
            AppUser user = await _userManager.FindByNameAsync(usernameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(usernameOrEmail);
            if (user == null)
                throw new NotFoundUserException();

            if (!(user == null))
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                if (result.Succeeded)
                {
                    Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);
                    await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 5);
                    return token;
                }
            }
            //return new LoginUserErrorCommandResponse()
            //{
            //    ErrorMessage = ""
            //};
            throw new AuthenticationErrorException();
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
           AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u=>u.RefreshToken==refreshToken);
            if (user != null && user?.RefreshTokenEndDate>DateTime.UtcNow)
            {
                Token token = _tokenHandler.CreateAccessToken(15);
                await _userService.UpdateRefreshToken(token.RefreshToken,user, token.Expiration, 15);
                return token;
            }
            else
                throw new NotFoundUserException();
        }
    }
}
