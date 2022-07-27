using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Service
{
    public class AccessTokenGenerator
    {
        private IConfiguration _configuration;
        public AccessTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtSecurityToken Generate(IdentityUser user)
        {
           var claims = new[]
           {
                new Claim("UserName", user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30), // expire in 30 minutes later
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));;

            return token;
        }
    }
}
