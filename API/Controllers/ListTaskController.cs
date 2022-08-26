using System.Threading.Tasks;
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
    }
}
