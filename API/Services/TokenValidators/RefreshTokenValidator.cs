using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Service.TokenValidators
{
    public class RefreshTokenValidator
    {
        private readonly IConfiguration _configuration;
        public RefreshTokenValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool Validate(string refreshToken)
        {
          
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:RefreshTokenSecret"])),
                ValidIssuer = _configuration["AuthSettings:Issuer"],
                ValidAudience = _configuration["AuthSettings:Audience"],
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}
