using Microsoft.EntityFrameworkCore;
using TaskFlow.Models; // Ensure this matches your models namespace

namespace TaskFlow.Data
{
    public class TaskFlowDbContext : DbContext
    {
        public TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options) : base(options)
        {
        }

        // Define DbSets (tables)
        public DbSet<TaskItem> TaskItems { get; set; } // Example entity
    }
}
