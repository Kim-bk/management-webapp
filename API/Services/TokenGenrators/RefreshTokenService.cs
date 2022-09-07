using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Domain.AggregateModels.UserAggregate;
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
            return await _refreshTokenRepository.FindAsync(tk => tk.Token == token);
        }
        public async Task Delete(string tokenId)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                _refreshTokenRepository.DeleteExp(tk => tk.Id == tokenId);
                await _unitOfWork.CommitTransaction();
            }
            catch 
            {
                await _unitOfWork.RollbackTransaction();
            }
        }
    }
}
