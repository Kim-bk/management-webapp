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
            var rs = await _projectService.GetProject(projectId);
            return new OkObjectResult(new
            {
                rs.Message,
                rs.ListTasks
            });
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
                return Ok(res.Message);
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
                var rs = await _projectService.AddMemberToProject(projectId, model);
                return Ok(rs.Message);
            }
            return Forbid();
        }

        [Authorize]
        [HttpGet("list-task/{listTaskId:int}")]
        // api/project/list-task/{listTaskId}
        public async Task<IActionResult> GetListTask(int listTaskId)
        {
            var rs = await _projectService.GetListTask(listTaskId);
            return Ok(new OkObjectResult(new
            {
                rs.Message,
                rs.ProjectId,
                rs.Task,
            }));
        }

        [Authorize]
        [HttpPost("list-task")]
        // api/project/list-task
        public async Task<IActionResult> CreateListTask([FromBody] ListTaskRequest request)
        {
            var rs = await _projectService.CreateListTask(request);
            return Ok(rs.Message);
        }

        [Authorize]
        [HttpDelete("{projectId:int}/list-task/{listTaskId:int}")]
        // api/project/{projectId}/list-task/{listTaskId}
        public async Task<IActionResult> DeleteListTask(int projectId, int listTaskId)
        {
            var rs = await _projectService.DeleteListTask(projectId, listTaskId);
            return Ok(rs.Message);
        }

        [Authorize]
        [HttpGet]
        // api/project
        public async Task<IActionResult> GetAllProjects()
        {
             var rs = await _projectService.GetAllProjects();
             return Ok(rs);
        }

        [Authorize]
        [HttpDelete("{projectId:int}")]
        // api/project/{projectId}
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            var rs = await _projectService.DeleteProject(projectId);
            return Ok(rs.Message);
        }
    }
}
