using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Services.TokenService
{
    public interface ITokenHandler
    {
        Token CreateAccessToken(int expirationMinute, Domain.Entities.Identity.AppUser user);
        string CreateRefreshToken();

    }
}
  