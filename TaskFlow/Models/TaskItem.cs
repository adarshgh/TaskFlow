namespace TaskFlow.Models
{
    public class TaskItem
    {
        public int Id { get; set; }  // Primary Key
        public string Title { get; set; }  // Task Name
        public string Description { get; set; }  // Task Details
        public bool IsCompleted { get; set; }  // Task Status
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Timestamp
    }
}
