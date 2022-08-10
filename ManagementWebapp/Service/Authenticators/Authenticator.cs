using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Domain.DTOs.Responses;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Service.TokenGenratorServices;

namespace Service.Authenticators
{
    public class Authenticator
    {
        private readonly AccessTokenService _accessTokenGenerator;
        private readonly RefreshTokenService _refreshTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Authenticator(AccessTokenService accessTokenGenerator, IUnitOfWork unitOfWork,
                        RefreshTokenService refreshTokenGenerator, 
                        IRefreshTokenRepository refreshTokenRepository)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthenticatedUserResponse> Authenticate(ApplicationUser user)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var accessToken = _accessTokenGenerator.Generate(user);
                var refreshToken = _refreshTokenGenerator.Generate();

                RefreshToken refreshTokenDTO = new RefreshToken()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(refreshToken),
                    UserId = user.Id
                };

                await _refreshTokenRepository.Create(refreshTokenDTO);
                await _unitOfWork.CommitTransaction();

                return new AuthenticatedUserResponse()
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                    RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken),
                };
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                return new AuthenticatedUserResponse()
                {
                    AccessToken = e.ToString(),
                };
            }
            
        }
    }
}
