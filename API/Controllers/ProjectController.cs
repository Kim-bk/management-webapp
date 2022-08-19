﻿using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IRoleService _roleService;
        public ProjectController(IProjectService projectService, IRoleService roleService)
        {
            _projectService = projectService;
            _roleService = roleService;
        }

        [Authorize]
        [HttpGet("{projectId:int}/list-task")]
        // api/project/{projectId}/list-task
        public async Task<IActionResult> GetListTasks(int projectId)
        {
            if (ModelState.IsValid)
            {
                var rs = await _projectService.GetListTasks(projectId);
                if (rs.IsSuccess)
                {
                    return new OkObjectResult(new
                    {
                        rs.Message,
                        rs.ListTasks
                    });
                }
            }
            return BadRequest("Invalid some properties!");
        }

        [Authorize]
        [HttpPost]
        // api/project
        public async Task<IActionResult> Create([FromBody] ProjectRequest model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var access = await _roleService.CheckUserRole(userId, "Admin");
            if (access)
            {
                var rs = await _projectService.CreateUserProject(userId, model);
                if (rs.IsSuccess)
                {
                    return Ok(rs.Message);
                }
                return BadRequest("Invalid some properties!");
            }
            return Forbid();
        }

        [Authorize]
        [HttpPost("{projectId:int}/user")]
        // api/project/{projectId}/member
        public async Task<IActionResult> AddMemberToProject(int projectId, [FromBody] ProjectRequest model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var access = await _roleService.CheckUserRole(userId, "Admin");
            if (access)
            {
                if (ModelState.IsValid)
                {
                    var rs = await _projectService.AddMemberToProject(projectId, model);
                    if (rs.IsSuccess)
                    {
                        return Ok(rs.Message);
                    }
                }
                return BadRequest("Invalid some properties!");
            }
            return Forbid();
        }

        [Authorize]
        [HttpPost("{projectId:int}/list-task")]
        // api/project/{projectId}/list-task
        public async Task<IActionResult> CreateListTask(int projectId, [FromBody] CommonRequest model)
        {
            if (ModelState.IsValid)
            {
                var rs = await _projectService.CreateListTask(projectId, model);
                if (rs.IsSuccess)
                {
                    return Ok(rs.Message);
                }
            }
            return BadRequest("Invalid some properties!");
        }
    }
}