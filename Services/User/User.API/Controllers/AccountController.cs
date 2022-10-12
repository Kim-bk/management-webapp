using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs.Requests;
using API.DTOs.Responses;
using API.IntegrationEvents;
using Domain.AggregateModels.UserAggregate;
using EventBus.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HttpClient _client;

        public AccountController(IAccountService accountSerivce, UserManager<ApplicationUser> userManager)
        {
            _accountService = accountSerivce;
            _userManager = userManager;
            _client = new HttpClient();
        }

        // api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            var res = await _accountService.Register(model);
            return Ok(res);
        }

        // api/account/login 
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
             var user = await _accountService.Login(model);

            // 1. Call http to Auth Service
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:5000/api/auth/" + user.Id)
            };

            var response = await _client.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            // 2. Read file as json
            var jsonContent = JsonConvert.DeserializeObject<object>(responseContent);

            // 3. Return access token vs refresh Token
            return Ok(jsonContent);
        }

        [Authorize]
        [HttpPost("logout")]
        // api/account/logout
        public async Task<IActionResult> Logout()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rs = await _accountService.Logout(userId);
            return Ok(rs.Message);
        }

        [Authorize]
        [HttpPut]
        // api/account
        public async Task<IActionResult> Update([FromBody] UserRequest req)
        {
            var rs = await _accountService.Update(req);
            return Ok(rs);
        }

        // Call http method to refresh access token
       /* [Authorize]
        [HttpPost("refresh")]*/
        // api/account/refresh
       /* public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
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
        }*/
    }
}
