using System.Security.Claims;
using System.Threading.Tasks;
using Domain.DTOS.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

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
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (ModelState.IsValid)
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
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                var res = await _accountService.UserLoginAsync(model);
                if (res != null)
                {
                    return Ok(res);
                }
            }
            return BadRequest("Some properties is not valid!"); // error code 400
        }

        // api/account/project
        [Authorize]
        [HttpGet("project")]
        public IActionResult GetUserProjects()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var res = _accountService.GetUserProjects(userId);
            if (res.IsSuccess)
            {
                return new OkObjectResult(new
                {
                    res.Message,
                    res.Projects
                });
            }
            return BadRequest("Some properties is not valid!"); // error code 400
        }

        // api/account/logout
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var res = await _accountService.UserLogout();
            if (res.IsSuccess)
            {
                return Ok(res);
            }
           
            return BadRequest("Can not log out!"); // error code 400
        }
    }
}
