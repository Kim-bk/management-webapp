using System.Security.Claims;
using System.Threading.Tasks;
using API.Entities;
using Domain.Intefaces.Services;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountSerivce)
        {
            _accountService = accountSerivce;
        }

        // api/account/register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if(ModelState.IsValid)
            {
                var res = await _accountService.RegisterUserAsync(model);
                if (res.IsSuccess)
                {
                    return Ok(res);
                }
            }

            return BadRequest("Some properties is not valid!"); // status code : 400
        }

        // api/account/login 
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if(ModelState.IsValid)
            {
                var res = await _accountService.UserLoginAsync(model);
                if(res.IsSuccess)
                {
                    return Ok(res);
                }
            }
            return BadRequest("Some properties is not valid!"); // error code 400
        }

        // api/account/project
        [Authorize]
        [HttpGet("Project")]
        public async Task<IActionResult> GetUserProjects()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var res = await _accountService.GetUserProjects(userId);
            if(res != null)
            {
                return Ok(res);
            }
            return BadRequest("Some properties is not valid!"); // error code 400
        }

        // api/account/project
        [Authorize]
        [HttpPost("Project")]
        public async Task<IActionResult> CreateUserProject([FromBody] ProjectRequest model)
        {
            if(ModelState.IsValid)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var res = await _accountService.CreateUserProject(model, userId);
                if (res.IsSuccess)
                {
                    return Ok(res);
                }
            }
            return BadRequest("Some properties is not valid!"); // error code 400
        }

        // api/account/logout
        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var res = await _accountService.UserLogout();
            if(res.IsSuccess)
            {
                return Ok(res);
            }
           
            return BadRequest("Can not log out!"); // error code 400
        }
    }
}
