using ETicaretAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUsers.LoginUser
{
    public class LoginUserCommandResponse
    {
    }

    // Burada response nesnesini duruma gore ikiye bolduk

    public class LoginUserSuccessCommandResponse:LoginUserCommandResponse
    {
        public Token Token{ get; set; }
    }
    public class LoginUserErrorCommandResponse : LoginUserCommandResponse 
    {
        public string ErrorMessage{ get; set; }
    }

}
