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
            if (ModelState.IsValid)
            {
                var rs = await _taskService.GetTask(id);
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
        [HttpPost("label")]
        // api/task/label
        public async Task<IActionResult> AddLabelToTask([FromBody] LabelRequest model)
        {
            if (ModelState.IsValid)
            {
                var rs = await _taskService.AddLabelToTask(model);
                if (rs.IsSuccess)
                {
                    return Ok(rs.Message);
                }
            }
            return BadRequest("Some properties is not valid!");
        }

        [Authorize]
        [HttpDelete("label")]
        // api/task/label
        public async Task<IActionResult> RemoveLabelInTask([FromBody] LabelRequest model)
        {
            if (ModelState.IsValid)
            {
                var rs = await _taskService.RemoveLabelInTask(model);
                if (rs.IsSuccess)
                {
                    return Ok(rs.Message);
                }
            }
            return BadRequest("Some properties is not valid!");
        }

        [Authorize]
        [HttpPost("todo")]
        // api/task/to-do
        public async Task<IActionResult> AddToDoToTask([FromBody] ToDoRequest model)
        {
            if (ModelState.IsValid)
            {
                var rs = await _taskService.AddToDoToTask(model);
                if (rs.IsSuccess)
                {
                    return Ok(rs.Message);
                }
            }

            return BadRequest("Some properties is not valid!");
        }

        [Authorize]
        [HttpPost("todo/todo-items")]
        // api/task/todo/todo-items
        public async Task<IActionResult> ManageToDoItems([FromBody] ToDoRequest model)
        {
            if (ModelState.IsValid)
            {
                var rs = await _taskService.ManageToDoItems(model);
                if (rs.IsSuccess)
                {
                    return Ok(rs.Message);
                }
            }

            return BadRequest("Some properties is not valid!");
        }

        [Authorize]
        [HttpPost("{taskId:int}/member")]
        // api/task/{taskId}/member
        public async Task<IActionResult> AssignMember(int taskId, [FromBody] CommonRequest model)
        {
            if (ModelState.IsValid)
            {
                var rs = await _taskService.AssignMember(taskId, model.UserId);
                if (rs.IsSuccess)
                {
                    return Ok(rs.Message);
                }
            }

            return BadRequest("Some properties is not valid!");
        }
    }
}
