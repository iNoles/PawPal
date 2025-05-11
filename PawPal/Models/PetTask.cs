using System;

namespace PawPal.Models;

public class PetTask
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string? Notes { get; set; }
    public bool IsCompleted { get; set; } = false;
    public int PetId { get; set; } // Foreign key to Pet

    public bool IsRecurring { get; set; }
    public string RecurrenceType { get; set; } = string.Empty;
    public int RecurrenceInterval { get; set; } = 1;
    public DateTime EndDate { get; set; }

    public Pet Pet { get; set; } = null!;
}
