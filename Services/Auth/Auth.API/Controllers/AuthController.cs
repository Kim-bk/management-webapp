﻿using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs.Requests;
using API.DTOs.Responses;
using Auth.API.Services;
using Auth.Domain.DTOs.Requests;
using Domain.AggregateModels.UserAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.TokenGenratorServices;
using Service.TokenValidators;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly Authenticator _authenticator;
        private readonly RefreshTokenService _refreshTokenService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthService authService, RefreshTokenValidator refreshTokenValidator,
                            Authenticator authenticator, RefreshTokenService refreshTokenService,
                            UserManager<ApplicationUser> userManager)
        {
            _refreshTokenService = refreshTokenService;
            _authenticator = authenticator;
            _refreshTokenValidator = refreshTokenValidator;
            _authService = authService;
            _userManager = userManager;
        }


        // api/auth/{userId}
        [HttpPost("{userId}")]
        public async Task<IActionResult> Authenticate(string userId)
        
        {
            var user = await _userManager.FindByIdAsync(userId);
             
            // 1. Generate user access token and refresh token
            var res = await _authenticator.Authenticate(user);
            return Ok(res);
        }


        [Authorize]
        [HttpPost("refresh")]
        // api/account/refresh
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Some properties is not valid!");
            }

            // 1. Check if refresh token is valid
            _refreshTokenValidator.Validate(refreshRequest.RefreshToken);

            // 2. Get refresh token by token
            var refreshTokenDTO = await _refreshTokenService.GetByToken(refreshRequest.RefreshToken);

            // 3. Delete that refresh token
            await _refreshTokenService.Delete(refreshTokenDTO.Id);

            // 4. Find user have that refresh token
            var user = await _userManager.FindByIdAsync(refreshTokenDTO.UserId);

            // 5. Generate new access token and refresh token to the user
            AuthenticatedUserResponse response = await _authenticator.Authenticate(user);

            return Ok(response);
        }
    }
}