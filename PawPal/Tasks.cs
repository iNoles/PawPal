using SQLite;

namespace PawPal;

public class Tasks
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string TaskName { get; set; } = string.Empty; // Task name

    public DateTime DueDate { get; set; } // Date when the task is due

    [Indexed] // To improve performance of foreign key queries
    public int PetId { get; set; }
}
