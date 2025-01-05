using System.Collections.ObjectModel;
using PawPal.Models;
using PawPal.Services;

namespace PawPal.ViewModel;

//TODO: Colored Indicatiors: Feeding = Green, Vet = Red, Grooming = Blue
//TODO: Tap to View/Edit Task
public class CalendarViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    public ObservableCollection<CalendarDay> VisibleCalendarDays { get; set; } = [];
    public string CurrentMonth => SelectedDate.ToString("MMMM yyyy");

    private DateTime _selectedDate;
    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            OnPropertyChanged();
            GenerateCalendar();
        }
    }

    private bool _isWeeklyView;
    public bool IsWeeklyView
    {
        get => _isWeeklyView;
        set
        {
            _isWeeklyView = value;
            OnPropertyChanged();
            GenerateCalendar();
        }
    }

    public Command ToggleViewCommand { get; }
    public Command NavigatePreviousCommand { get; }
    public Command NavigateNextCommand { get; }

    public CalendarViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        SelectedDate = DateTime.Today;
        ToggleViewCommand = new Command(() => IsWeeklyView = !IsWeeklyView);
        NavigatePreviousCommand = new Command(() => ChangeDate(-1));
        NavigateNextCommand = new Command(() => ChangeDate(1));

        GenerateCalendar();
    }

    private void ChangeDate(int offset)
    {
        SelectedDate = IsWeeklyView ? SelectedDate.AddDays(offset * 7) : SelectedDate.AddMonths(offset);
    }

    private void GenerateCalendar()
    {
        VisibleCalendarDays.Clear();
        if (IsWeeklyView)
        {
            foreach (var day in GenerateWeeklyCalendarDays())
                VisibleCalendarDays.Add(day);
        }
        else
        {
            foreach (var day in GenerateMonthlyCalendarDays())
                VisibleCalendarDays.Add(day);
        }
    }

    private ObservableCollection<CalendarDay> GenerateWeeklyCalendarDays()
    {
        var days = new ObservableCollection<CalendarDay>();
        var startOfWeek = SelectedDate.AddDays(-(int)SelectedDate.DayOfWeek);
        var endOfWeek = startOfWeek.AddDays(6);
        var tasks = _databaseService.GetTasksForMonthAsync(startOfWeek);

        for (int i = 0; i < 7; i++)
        {
            var currentDate = startOfWeek.AddDays(i);
            var dailyTasks = tasks.Where(t => t.ScheduledDate.Date == currentDate).ToList();

            days.Add(new CalendarDay
            {
                Date = currentDate,
                HasTasks = dailyTasks.Any(),
                IsCurrentMonth = currentDate.Month == SelectedDate.Month,
            });
        }
        return days;
    }

    private ObservableCollection<CalendarDay> GenerateMonthlyCalendarDays()
    {
        var days = new ObservableCollection<CalendarDay>();
        var firstDayOfMonth = new DateTime(SelectedDate.Year, SelectedDate.Month, 1);
        var firstDayOfGrid = firstDayOfMonth.AddDays(-(int)firstDayOfMonth.DayOfWeek);
        var lastDayOfGrid = firstDayOfMonth.AddMonths(1).AddDays(-1).AddDays(6 - (int)firstDayOfMonth.AddMonths(1).DayOfWeek);

        var tasks = _databaseService.GetTasksForMonthAsync(SelectedDate);

        for (var date = firstDayOfGrid; date <= lastDayOfGrid; date = date.AddDays(1))
        {
            var dailyTasks = tasks.Where(t => t.ScheduledDate.Date == date).ToList();

            days.Add(new CalendarDay
            {
                Date = date,
                HasTasks = dailyTasks.Count != 0,
                IsCurrentMonth = date.Month == SelectedDate.Month,
            });
        }
        return days;
    }

}
