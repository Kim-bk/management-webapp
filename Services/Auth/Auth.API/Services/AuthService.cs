using API.DTOs.Responses;
using Domain.AggregateModels.UserAggregate;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Service;
using Service.TokenGenratorServices;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Auth.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly AccessTokenService _accessTokenGenerator;
        private readonly RefreshTokenService _refreshTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(AccessTokenService accessTokenGenerator, IUnitOfWork unitOfWork,
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
                //await _unitOfWork.BeginTransaction();

                // 1. Generate access vs refresh token
                var accessToken = _accessTokenGenerator.Generate(user);
                var refreshToken = _refreshTokenGenerator.Generate();

                // 2. Init refresh token properties
                string refreshTokenId = Guid.NewGuid().ToString();
                string refreshTokenHandler = new JwtSecurityTokenHandler().WriteToken(refreshToken);

                // 3. Create and save user refresh token
                var userRefreshToken = user.CreateRefreshToken(refreshTokenId, refreshTokenHandler);
                await _refreshTokenRepository.AddAsync(userRefreshToken);

                await _unitOfWork.CommitTransaction();

                // 3. Return two tokens (AccessToken vs RefreshToken)
                // return new JwtSecurityTokenHandler().WriteToken(accessToken);
                return new AuthenticatedUserResponse()
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                    RefreshToken = refreshTokenHandler
                };
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}
