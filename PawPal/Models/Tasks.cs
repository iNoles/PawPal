using SQLite;

namespace PawPal.Models;

public class Tasks
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string TaskName { get; set; } = string.Empty; // Task name

    public DateTime DueDate { get; set; } // Date when the task is due

    public bool IsCompleted { get; set; } = false;

    public string Category { get; set; } = "General"; // Category of the task

    [Indexed] // To improve performance of foreign key queries
    public int PetId { get; set; }
}
