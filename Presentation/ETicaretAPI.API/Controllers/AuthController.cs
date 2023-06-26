using ETicaretAPI.Application.Features.Commands.AppUsers.GoogleLogin;
using ETicaretAPI.Application.Features.Commands.AppUsers.LoginUser;
using ETicaretAPI.Application.Features.Commands.AppUsers.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IMediator _mediatr;

        public AuthController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin([FromBody] RefreshTokenCommandRequest refreshTokenCommandRequest)
        {
            RefreshTokenCommandResponse response = await _mediatr.Send(refreshTokenCommandRequest);
            return Ok(response);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
        {
            LoginUserCommandResponse response = await _mediatr.Send(loginUserCommandRequest);
            return Ok(response);
        }
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommandRequest googleLoginCommandRequest)
        {
            GoogleLoginCommandResponse response = await _mediatr.Send(googleLoginCommandRequest);
            return Ok(response);
        }
    }
}
