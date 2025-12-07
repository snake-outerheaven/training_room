namespace TodoApi.Models;

public class TodoItem
{
        public int Id { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
}