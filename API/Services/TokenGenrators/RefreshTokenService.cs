using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Service.TokenGenratorServices
{
    public class RefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly TokenGenerator _tokenGenerator;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, TokenGenerator tokenGenerator,
                                IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenGenerator = tokenGenerator;
            _configuration = configuration;
        }

        public JwtSecurityToken Generate()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:RefreshTokenSecret"]));
            var issuer = _configuration["AuthSettings:Issuer"];
            var audience = _configuration["AuthSettings:Audience"];
            var expires = DateTime.UtcNow.AddHours(256); // expire in 256 hours later

            return _tokenGenerator.GenerateToken(key, issuer, audience, expires);
        }

        public async Task<RefreshToken> GetByToken(string token)
        {
            return await _refreshTokenRepository.FindByToken(token);
        }
        public async System.Threading.Tasks.Task Delete(string tokenId)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                await _refreshTokenRepository.Delete(tokenId);
                await _unitOfWork.SaveChangesAsync();
            }
            catch 
            {
                await _unitOfWork.RollbackTransaction();
            }
        }
    }
}
