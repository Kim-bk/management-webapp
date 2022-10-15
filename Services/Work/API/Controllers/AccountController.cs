using System.Security.Claims;
using System.Threading.Tasks;
using Domain.AggregateModels.UserAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IAccountService accountSerivce, UserManager<ApplicationUser> userManager)
        {
            _accountService = accountSerivce;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet("project")]
        // api/account/project
        public async Task<IActionResult> GetUserProjects() 
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var res = await _accountService.GetUserProjects(userId);
            return new OkObjectResult(new
            {
                res.Message,
                res.Projects
            });
        }
    }
}
