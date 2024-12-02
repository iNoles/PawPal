namespace PawPal.Models;

public class CalendarDay
{
    public DateTime Date { get; set; }
    public bool HasTasks { get; set; } // Indicates if tasks/events exist for this day
    public bool IsCurrentMonth { get; set; } // For styling non-current month days in a grid
}
