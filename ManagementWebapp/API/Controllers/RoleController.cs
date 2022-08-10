using System.Security.Claims;
using System.Threading.Tasks;
using Domain.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        // api/role
        public async Task<IActionResult> GetAllRoles()
        {
            // 1. Check role if admin
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var access = await _roleService.CheckUserRole(userId, "Admin");
            if (access)
            {
                if (ModelState.IsValid)
                {
                    var rs = await _roleService.GetAllRoles();
                    if (rs != null)
                    {
                        return Ok(rs);
                    }
                }
                return BadRequest("Some properties is not valid!");
            }
            return Forbid();
        }

        [HttpPost]
        // api/role
        public async Task<IActionResult> CreateRole([FromBody] RoleRequest request)
        {
            // 1. Check role if admin
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var access = await _roleService.CheckUserRole(userId, "Admin");
            if (access)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult rs = await _roleService.CreateRole(request.RoleName);
                    if (rs.Succeeded)
                    {
                        return Ok("Create role success!");
                    }
                }
                return BadRequest("Some properties is not valid!");
            }
            return Forbid();
        }

        [HttpPut]
        // api/role
        public async Task<IActionResult> AddRoleToUser([FromBody] RoleRequest request)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var access = await _roleService.CheckUserRole(userId, "Admin");
            if (access)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult rs = await _roleService.AddRoleToUser(request);
                    if (rs.Succeeded)
                    {
                        return Ok("Add role to user success!");
                    }
                }
                return BadRequest("Some properties is not valid!");
            }
            return Forbid();
        }

        [HttpDelete]
        // api/role
        public async Task<IActionResult> DeleteRole([FromBody] RoleRequest request)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var access = await _roleService.CheckUserRole(userId, "Admin");
            if (access)
            {
                if (ModelState.IsValid)
                {
                    var rs = await _roleService.DeleteRole(userId, request.RoleName);
                    if (rs)
                    {
                        return Ok("Delete role success!");
                    }
                }
                return BadRequest("Some properties is not valid!");
            }
            return Forbid();
        }
    }
}
