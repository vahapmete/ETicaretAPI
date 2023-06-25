using ETicaretAPI.Application.Services.AppUser.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Services.AppUser
{
    public interface IAuthService:IInternalAuthentication,IExternalAuthentication
    {
    }
}
