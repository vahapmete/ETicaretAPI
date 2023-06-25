using ETicaretAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Services.AppUser.Authentications
{
    public interface IExternalAuthentication
    {
        Task<Token> GoogleLoginAsync(string idToken,int accessTokenLifeTime);
    }
}
