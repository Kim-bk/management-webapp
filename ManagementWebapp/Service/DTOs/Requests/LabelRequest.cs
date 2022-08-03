namespace Service.DTOs.Requests
{
    public class LabelRequest
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
    }
}
