
namespace Domain.DTOs.Responses
{
    public class TaskManagerResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public TaskDTO Task { get; set; }
    }
}
