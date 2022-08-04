using System.Security.Claims;
using System.Threading.Tasks;
using Domain.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
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
                        rs.Project
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
        [HttpPost("user")]
        // api/project/user
        public async Task<IActionResult> AddMemberToProject([FromBody] ProjectRequest model)
        {
            if (ModelState.IsValid)
            {
                var rs = await _projectService.AddMemberToProject(model);
                if (rs.IsSuccess)
                {
                    return Ok(rs);
                }
            }
            return BadRequest("Invalid some properties!");
        }

        [Authorize]
        [HttpPost("list-task")]
        // api/project/list-task
        public async Task<IActionResult> CreateListTask([FromBody] CommonRequest model)
        {
            if (ModelState.IsValid)
            {
                var rs = await _projectService.CreateListTask(model);
                if (rs.IsSuccess)
                {
                    return Ok(rs);
                }
            }
            return BadRequest("Invalid some properties!");
        }
    }
}
