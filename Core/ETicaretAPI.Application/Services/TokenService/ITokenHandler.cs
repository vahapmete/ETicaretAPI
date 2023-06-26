using ETicaretAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Services.TokenService
{
    public interface ITokenHandler
    {
        Token CreateAccessToken(int expirationMinute);
        string CreateRefreshToken();
    }
}
  