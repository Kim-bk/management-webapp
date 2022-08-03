using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Requests;
using Service.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetInfoTask(int id)
        {
            if (ModelState.IsValid)
            {
                var rs = await _taskService.GetAllComponentsOfTask(id);
                if (rs.IsSuccess)
                {
                    return Ok(rs);
                }
            }
            return BadRequest("Some properties is not valid!");
        }

        [Authorize]
        [HttpPost("label")]
        // api/task/label
        public async Task<IActionResult> AddLabel([FromBody] LabelRequest model)
        {
            if (ModelState.IsValid)
            {
                var rs = await _taskService.AddLabel(model);
                if (rs.IsSuccess)
                {
                    return Ok(rs);
                }
            }
            return BadRequest("Some properties is not valid!");
        }

        [Authorize]
        [HttpDelete("label")]
        // api/task/label
        public async Task<IActionResult> RemoveLabel([FromBody] LabelRequest model)
        {
            if (ModelState.IsValid)
            {
                var rs = await _taskService.RemoveLabel(model);
                if (rs.IsSuccess)
                {
                    return Ok(rs);
                }
            }
            return BadRequest("Some properties is not valid!");
        }

        [Authorize]
        [HttpPost("todo")]
        // api/task/to-do
        public async Task<IActionResult> AddToDo([FromBody] ToDoRequest model)
        {
            if (ModelState.IsValid)
            {
                var rs = await _taskService.AddToDo(model);
                if (rs.IsSuccess)
                {
                    return Ok(rs);
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
                    return Ok(rs);
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
                    return Ok(rs);
                }
            }

            return BadRequest("Some properties is not valid!");
        }
    }
}
