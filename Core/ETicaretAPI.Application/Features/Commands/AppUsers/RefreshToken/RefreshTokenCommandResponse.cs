using ETicaretAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUsers.RefreshToken
{
    public class RefreshTokenCommandResponse
    {
        public Token Token { get; set; }
    }
}
