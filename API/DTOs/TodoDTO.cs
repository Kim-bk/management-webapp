namespace API.DTOs
{
    public class TodoDTO
    {
        public int TodoId { get; set; }
        public bool? IsDone { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
    }
}
