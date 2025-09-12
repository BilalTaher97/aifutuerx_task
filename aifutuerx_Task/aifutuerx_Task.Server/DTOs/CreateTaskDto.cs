namespace aifutuerx_Task.Server.DTOs
{
    public class CreateTaskDto
    {

        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int StatusId { get; set; }

    }
}
