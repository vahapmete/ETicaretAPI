using ETicaretAPI.Application.Features.Commands.AppUsers.CreateUser;
using ETicaretAPI.Application.Features.Commands.AppUsers.GoogleLogin;
using ETicaretAPI.Application.Features.Commands.AppUsers.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Formats.Asn1;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController : ControllerBase
    {
        private IMediator _mediatr;

        public AppUsersController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }
        public async  Task<IActionResult> CreateAppUser(CreateAppUserCommandRequest createAppUserCommandRequest)
        {
            CreateAppUserCommandResponse response= await _mediatr.Send(createAppUserCommandRequest);
            return Ok(response);
        }
       
    }
}
