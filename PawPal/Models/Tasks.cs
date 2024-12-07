using SQLite;

namespace PawPal.Models;

public class Tasks
{
    [PrimaryKey, AutoIncrement]
    // Unique identifier for the task
    public int Id { get; set; }

    // Title or type of the task (e.g., "Feeding", "Vet Appointment")
    public string Title { get; set; } = string.Empty;

    // Date and time when the task is scheduled
    public DateTime ScheduledDate { get; set; }

    // Optional notes or additional details about the task
    public string? Notes { get; set; }

    // Indicates if the task has been completed
    public bool IsCompleted { get; set; } = false;

    [Indexed]
    // To improve performance of foreign key queries
    // Foreign key to associate the task with a specific pet
    public int PetId { get; set; }
}
