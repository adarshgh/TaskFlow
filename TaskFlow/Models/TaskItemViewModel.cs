using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Models
{
    public class TaskItemViewModel
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? UserId { get; set; } // ✅ Must be nullable to allow unassigned tasks.
    }


}
