namespace API.DTOs.Requests
{ 
    public class TaskRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ListTaskId { get; set; }
    }
}
