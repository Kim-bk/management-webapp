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
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        // api/task/{id}
        public async Task<IActionResult> GetTask(int id)
        {
            var rs = await _taskService.GetTask(id);
            return Ok(new OkObjectResult(new { 
                rs.Message,
                rs.Task,
            }));
        }

        [Authorize]
        [HttpPost("label")]
        // api/task/label
        public async Task<IActionResult> AddLabelToTask([FromBody] LabelRequest model)
        {
            var rs = await _taskService.AddLabelToTask(model);
            return Ok(rs.Message);
        }

        [Authorize]
        [HttpDelete("label")]
        // api/task/label
        public async Task<IActionResult> RemoveLabelInTask([FromBody] LabelRequest model)
        {
            var rs = await _taskService.RemoveLabelInTask(model);
            return Ok(rs.Message);
        }

        [Authorize]
        [HttpPost("todo")]
        // api/task/to-do
        public async Task<IActionResult> AddToDoToTask([FromBody] ToDoRequest model)
        {
            var rs = await _taskService.AddToDoToTask(model);
            return Ok(rs.Message);
        }

        [Authorize]
        [HttpPut("todo")]
        // api/task/todo
        public async Task<IActionResult> UpdateTodoItems([FromBody] ToDoRequest model)
        {
            var rs = await _taskService.UpdateTodoItems(model);
            return Ok(rs.Message);
        }

        [Authorize]
        [HttpPost("{taskId:int}/member")]
        // api/task/{taskId}/member
        public async Task<IActionResult> AssignMember(int taskId, [FromBody] CommonRequest model)
        {
            var rs = await _taskService.AssignMember(taskId, model.UserId);
            return Ok(rs.Message);
        }

        [Authorize]
        [HttpPut("{taskId:int}")]
        // api/task/{taskId:int}
        public async Task<IActionResult> MoveTask(int taskId, [FromBody] MoveTaskRequest request)
        {
            var rs = await _taskService.MoveTask(taskId, request);
            return Ok(rs);
        }
        
        [Authorize]
        [HttpPost]
        // api/task
        public async Task<IActionResult> AddTask(TaskRequest request)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rs = await _taskService.CreateTask(request, userId);
            return Ok(rs.Message);
        }
    }
}
