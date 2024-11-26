namespace PawPal;

public class Tasks
{
    public int Id { get; set; }

    public string TaskName { get; set; } = string.Empty; // Task name (non-nullable)

    public DateTime DueDate { get; set; } // Date when the task is due (non-nullable)

    public required Pet Pet { get; set; } // Navigation property to the related pet (non-nullable, enforced by required keyword in .NET 9)
}
