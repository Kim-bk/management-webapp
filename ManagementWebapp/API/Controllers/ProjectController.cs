using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTOs.Requests;
using Service.DTOS.Requests;
using Service.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]-management")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [Authorize]
        [HttpPost]
        // api/project/create
        public async Task<IActionResult> Create([FromBody] ProjectRequest model)
        {
            if(ModelState.IsValid)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var rs = await _projectService.CreateUserProject(userId, model);
                if (rs.IsSuccess)
                {
                    return Ok(rs);
                }
            }
          
            return BadRequest("Invalid some properties!");
        }

        [Authorize]
        [HttpPost("user/{userId}/project/{projectId:int}")]
        // api/project/create
        public async Task<IActionResult> AddMemberToProject(string userId, int projectId)
        {
            if (ModelState.IsValid)
            {
                var rs = await _projectService.AddMemberToProject(userId, projectId);
                if (rs.IsSuccess)
                {
                    return Ok(rs);
                }
            }

            return BadRequest("Invalid some properties!");
        }

        [Authorize]
        [HttpPost("list-task")]
        public async Task<IActionResult> CreateListTask(int projectId, [FromBody] TitleRequest model)
        {
            if (ModelState.IsValid)
            {
                var rs = await _projectService.CreateListTask(model.Title, projectId);
                if (rs.IsSuccess)
                {
                    return Ok(rs);
                }
            }
            return BadRequest("Invalid some properties!");
        }
    }
}
