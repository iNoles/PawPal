using System;

namespace PawPal;

public class CalendarDay
{
    public DateTime Date { get; set; }
    public bool HasTasks => Tasks.Count != 0;
    public List<Tasks> Tasks { get; set; } = [];
}
