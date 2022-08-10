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
    public class ListTaskController : ControllerBase
    {
        private readonly IListTaskService _listTaskService;
        public ListTaskController(IListTaskService listTaskService)
        {
            _listTaskService = listTaskService;
        }
        
        [Authorize]
        [HttpPost("{listTaskId:int}/task")]
        // api/listtask-management/task
        public async Task<IActionResult> CreateTask([FromBody] CommonRequest model, int listTaskId)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var rs = await _listTaskService.AddTaskToList(listTaskId, model, userId);
                if (rs.IsSuccess)
                {
                    return Ok(rs.Message);
                }
            }
            return BadRequest("Some properties is not valid!");
        }

        [Authorize]
        [HttpGet("{listTaskId:int}/task")]
        // api/listtask/{listTaskId}/task
        public async Task<IActionResult> GetAllTasks(int listTaskId)
        {
            if (ModelState.IsValid)
            {
                var rs = await _listTaskService.GetAllTasks(listTaskId);
                if (rs.IsSuccess)
                {
                    return Ok(new OkObjectResult(new {
                        rs.Message,
                        rs.Task,
                    }));
                }
            }
            return BadRequest("Some properties is not valid!");
        }

        [Authorize]
        [HttpPost("managed-task")]
        // api/listtask-management/managed-task
        public async Task<IActionResult> MoveTask([FromBody] TaskRequest model)
        {
            if (ModelState.IsValid)
            {
                var rs = await _listTaskService.MoveTask(model);
                if (rs.IsSuccess)
                {
                    return Ok(rs);
                }
            }
            return BadRequest("Some properties is not valid!");
        }
    }
}
