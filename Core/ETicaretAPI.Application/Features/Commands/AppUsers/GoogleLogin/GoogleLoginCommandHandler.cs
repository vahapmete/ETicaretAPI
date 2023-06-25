using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.Services.AppUser;
using ETicaretAPI.Application.Services.TokenService;
using ETicaretAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUsers.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
    {

        readonly IAuthService _authService;

        public GoogleLoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
           Token token = await _authService.GoogleLoginAsync(request.IdToken,15);
            return new GoogleLoginCommandResponse()
            {
                Token = token,
            };
        }
    }
}
 