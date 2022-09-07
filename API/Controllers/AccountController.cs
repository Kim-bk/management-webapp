using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs.Requests;
using API.DTOs.Responses;
using Domain.AggregateModels.UserAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Authenticators;
using Service.Interfaces;
using Service.TokenGenratorServices;
using Service.TokenValidators;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly Authenticator _authenticator;
        private readonly RefreshTokenService _refreshTokenService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IAccountService accountSerivce, RefreshTokenValidator refreshTokenValidator,
                            Authenticator authenticator, RefreshTokenService refreshTokenService,
                            UserManager<ApplicationUser> userManager)
        {
            _refreshTokenService = refreshTokenService;
            _authenticator = authenticator;
            _refreshTokenValidator = refreshTokenValidator;
            _accountService = accountSerivce;
            _userManager = userManager;
        }

        // api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                var res = await _accountService.Register(model);
                if (res.IsSuccess)
                {
                    return Ok(res);
                }
            }

            return BadRequest("Some properties is not valid!"); // status code : 400
        }

        // api/account/login 
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountService.Login(model);
                if (user != null)
                {
                    // 1. Get user access token and refresh token
                    var res = await _authenticator.Authenticate(user);
                    return Ok(res);
                }
            }
            return BadRequest("Some properties is not valid!"); // error code 400
        }
        
        [Authorize]
        [HttpGet("project")]
        // api/account/project
        public async Task<IActionResult> GetUserProjects() 
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var res = await _accountService.GetUserProjects(userId);
            if (res.IsSuccess)
            {
                return new OkObjectResult(new
                {
                    res.Message,
                    res.Projects
                });
            }
            return BadRequest(res); // error code 400
        }

        [Authorize]
        [HttpPost("logout")]
        // api/account/logout
        public async Task<IActionResult> Logout()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rs = await _accountService.Logout(userId);
            if (rs.IsSuccess)
            {
                return Ok(rs.Message);
            }
            return BadRequest(rs.Message);
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
            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
            if (!isValidRefreshToken)
            {
                return BadRequest("Invalid refresh token.");
            }

            // 2. Get refresh token by token
            var refreshTokenDTO = await _refreshTokenService.GetByToken(refreshRequest.RefreshToken);
            if (refreshTokenDTO == null)
            {
                return NotFound("Invalid refresh token.");
            }

            // 3. Delete that refresh token
            await _refreshTokenService.Delete(refreshTokenDTO.Id);

            var user = await _userManager.FindByIdAsync(refreshTokenDTO.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // 4. Generate new access token and refresh token
            AuthenticatedUserResponse response = await _authenticator.Authenticate(user);

            return Ok(response);
        }
    }
}
