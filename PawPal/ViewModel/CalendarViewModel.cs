using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PawPal.ViewModel;

public class CalendarViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;
    private ObservableCollection<CalendarDay> _calendarDays = new();
    private CalendarDay? _selectedDay;
    private DateTime _currentMonth = DateTime.Now;

    public ObservableCollection<CalendarDay> CalendarDays
    {
        get => _calendarDays;
        set => SetProperty(ref _calendarDays, value);
    }

    public CalendarDay? SelectedDay
    {
        get => _selectedDay;
        set => SetProperty(ref _selectedDay, value);
    }

    public DateTime CurrentMonth
    {
        get => _currentMonth;
        set
        {
            SetProperty(ref _currentMonth, value);
            LoadCalendarDays(); // Refresh days when month changes
        }
    }

    public ICommand NavigatePreviousMonthCommand { get; }
    public ICommand NavigateNextMonthCommand { get; }

    public CalendarViewModel()
    {
        _databaseService = new DatabaseService();

        NavigatePreviousMonthCommand = new Command(() =>
        {
            CurrentMonth = CurrentMonth.AddMonths(-1);
        });

        NavigateNextMonthCommand = new Command(() =>
        {
            CurrentMonth = CurrentMonth.AddMonths(1);
        });

        LoadCalendarDays();
    }

    private void LoadCalendarDays()
    {
        CalendarDays.Clear();

        var firstDayOfMonth = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 1);
        var startOffset = (int)firstDayOfMonth.DayOfWeek;
        var daysInMonth = DateTime.DaysInMonth(CurrentMonth.Year, CurrentMonth.Month);

        var tasksForMonth = _databaseService.GetTasksForMonth(CurrentMonth);

        for (int i = 0; i < startOffset; i++)
        {
            CalendarDays.Add(new CalendarDay { Date = firstDayOfMonth.AddDays(-startOffset + i) });
        }

        for (int day = 1; day <= daysInMonth; day++)
        {
            var date = new DateTime(CurrentMonth.Year, CurrentMonth.Month, day);
            var tasksForDay = tasksForMonth.Where(t => t.DueDate.Date == date).ToList();

            CalendarDays.Add(new CalendarDay
            {
                Date = date,
                Tasks = tasksForDay
            });
        }

        var remainingDays = 7 - (CalendarDays.Count % 7);
        if (remainingDays < 7)
        {
            for (int i = 0; i < remainingDays; i++)
            {
                CalendarDays.Add(new CalendarDay { Date = firstDayOfMonth.AddDays(daysInMonth + i) });
            }
        }
    }
}
