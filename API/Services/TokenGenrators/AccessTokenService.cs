using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.AggregateModels.UserAggregate;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.TokenGenratorServices;

namespace Service
{
    public class AccessTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;
        public AccessTokenService(IConfiguration configuration, TokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        public JwtSecurityToken Generate(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim("UserName", user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:AccessTokenSecret"]));
            var issuer =  _configuration["AuthSettings:Issuer"];
            var audience =  _configuration["AuthSettings:Audience"];
            var expires =  DateTime.UtcNow.AddHours(8); // expire in 8 hours later

            var token = _tokenGenerator.GenerateToken(key, issuer, audience, expires, claims);
            return token;
        }
    }
}
