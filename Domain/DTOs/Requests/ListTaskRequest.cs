namespace API.DTOs.Requests
{
    public class ListTaskRequest
    {
        public int ProjectId { get; set; }
        public string Title { get; set; }
    }
}
