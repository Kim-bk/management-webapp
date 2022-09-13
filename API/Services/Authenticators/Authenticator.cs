using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using API.DTOs.Responses;
using Domain.AggregateModels.UserAggregate;
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
                        RefreshTokenService refreshTokenGenerator, IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthenticatedUserResponse> Authenticate(ApplicationUser user)
        {
            try
            {
                // 1. Generate access vs refresh token
                var accessToken = _accessTokenGenerator.Generate(user);
                var refreshToken = _refreshTokenGenerator.Generate();

                // 2. Init refresh token properties
                string refreshTokenId = Guid.NewGuid().ToString();
                string refreshTokenHandler = new JwtSecurityTokenHandler().WriteToken(refreshToken);

                // 3. Create user refresh token
                var userRefreshToken = user.CreateRefreshToken(refreshTokenId, refreshTokenHandler);
                 _refreshTokenRepository.AddAsync(userRefreshToken);

                await _unitOfWork.CommitTransaction();

                // 3. Return two tokens
                return new AuthenticatedUserResponse()
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                    RefreshToken = refreshTokenHandler
                };
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw e;
            }
        }
    }
}
