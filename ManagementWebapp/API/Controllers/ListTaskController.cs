using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Requests;
using Service.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]-management")]
    [ApiController]
    public class ListTaskController : ControllerBase
    {
        private IListTaskService _listTaskService;
        public ListTaskController(IListTaskService listTaskService)
        {
            _listTaskService = listTaskService;
        }
        
        [Authorize]
        [HttpPost("task")]
        // api/listtask-management/task?listTaskId
        public async Task<IActionResult> CreateTask(int listTaskId, [FromBody] TitleRequest model)
        {
            if (ModelState.IsValid)
            {
                var rs = await _listTaskService.AddTaskToList(model.Title, listTaskId);
                if (rs.IsSuccess)
                {
                    return Ok(rs);
                }
            }
            return BadRequest("Some properties is not valid!");
        }

        [Authorize]
        [HttpGet("task")]
        // api/listtask-management/task/?listTaskId
        public async Task<IActionResult> GetAll(int listTaskId)
        {
            if (ModelState.IsValid)
            {
                var rs = await _listTaskService.GetAllTasks(listTaskId);
                if (rs.IsSuccess)
                {
                    return Ok(rs);
                }
            }
            return BadRequest("Some properties is not valid!");
        }
    }
}
