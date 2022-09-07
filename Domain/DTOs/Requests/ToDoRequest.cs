namespace API.DTOs.Requests
{
    public class ToDoRequest
    {
        public int TaskId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public int? ParentId { get; set; }
    }
}
