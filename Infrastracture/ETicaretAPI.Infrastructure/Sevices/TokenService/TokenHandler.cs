using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.Services.TokenService;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Sevices.TokenService
{
    public class TokenHandler : ITokenHandler
    {
        IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Token CreateAccessToken(int expirationSecond)
        {
            Token token = new Token();
            SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(_configuration["TokenParameters:SecurityKey"]));
            SigningCredentials signingCredentials = new(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);
            token.Expiration = DateTime.UtcNow.AddSeconds(expirationSecond);
            JwtSecurityToken securityToken = new(audience: _configuration["TokenParameters:Audience"],
               issuer: _configuration["TokenParameters:Issuer"],
               expires: token.Expiration,
               notBefore: DateTime.UtcNow,
               signingCredentials: signingCredentials
                );
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(securityToken);
            token.RefreshToken = CreateRefreshToken();
            return token;
        }

        public string CreateRefreshToken()
        {
            byte[]number=new byte[32];
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }
    }
}
