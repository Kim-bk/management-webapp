using System;
using System.Threading.Tasks;
using API.DTOs.Responses;
using API.DTOs.Requests;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Domain.AggregateModels.UserAggregate;
using Auth.Domain.DTOs.Requests;
using API.Services;

namespace Auth.API.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(IUnitOfWork uniOfWork,
                    UserManager<ApplicationUser> userManager,
                    IRefreshTokenRepository refreshTokenRepository) : base(uniOfWork)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userManager = userManager;
        }



        public async Task<ApplicationUser> Login(LoginRequest model)
        {
            // 1. Validate input
            if (model == null)
            {
                throw new ArgumentNullException("Empty input");
            }

            // 2. Check username exists
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                // 3. Check password
                var password = await _userManager.CheckPasswordAsync(user, model.Password);
                if (password)
                {
                    return user;
                }
            }

            throw new NullReferenceException("Cant login!");
        }

    }
}
