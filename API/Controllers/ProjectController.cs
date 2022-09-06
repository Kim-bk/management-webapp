using System.Security.Claims;
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
        [HttpGet("{projectId:int}")]
        // api/project/{projectId}
        public async Task<IActionResult> GetProject(int projectId)
        {
            if (ModelState.IsValid)
            {
                var rs = await _projectService.GetProject(projectId);
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
                var res = await _projectService.CreateUserProject(userId, model);
                if (res.IsSuccess)
                {
                    return Ok(res.Message);
                }
                return BadRequest("Invalid some properties!");
            }
            return Forbid();
        }

        [Authorize]
        [HttpPost("{projectId:int}/user")]
        // api/project/{projectId}/user
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
        [HttpGet("list-task/{listTaskId:int}")]
        // api/project/list-task/{listTaskId}
        public async Task<IActionResult> GetListTask(int listTaskId)
        {
            if (ModelState.IsValid)
            {
                var rs = await _projectService.GetListTask(listTaskId);
                if (rs.IsSuccess)
                {
                    return Ok(new OkObjectResult(new
                    {
                        rs.Message,
                        rs.ProjectId,
                        rs.Task,
                    }));
                }
            }
            return BadRequest("Some properties is not valid!");
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

        [Authorize]
        [HttpDelete("{projectId:int}/list-task/{listTaskId:int}")]
        // api/project/{projectId}/list-task
        public async Task<IActionResult> DeleteListTask(int projectId, int listTaskId)
        {
            if (ModelState.IsValid)
            {
                var rs = await _projectService.DeleteListTask(projectId, listTaskId);
                if (rs.IsSuccess)
                {
                    return Ok(rs.Message);
                }
            }
            return BadRequest("Invalid some properties!");
        }

        [Authorize]
        [HttpGet]
        // api/project
        public async Task<IActionResult> GetAllProjects()
        {
            if (ModelState.IsValid)
            {
                var rs = await _projectService.GetAllProjects();
                if (rs.IsSuccess)
                {
                    return Ok(rs);
                }
            }
            return BadRequest("Invalid some properties!");
        }

        [Authorize]
        [HttpDelete("{projectId:int}")]
        // api/project/{projectId}
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            if (ModelState.IsValid)
            {
                var rs = await _projectService.DeleteProject(projectId);
                if (rs.IsSuccess)
                {
                    return Ok(rs.Message);
                }
            }
            return BadRequest("Invalid some properties!");
        }
    }
}
