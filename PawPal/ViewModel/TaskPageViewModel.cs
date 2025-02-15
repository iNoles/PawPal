namespace PawPal.ViewModel;

using System.Collections.ObjectModel;
using System.Windows.Input;
using PawPal.Models;
using PawPal.Services;

public class TaskPageViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;
    private readonly bool _isEditMode;

    private Pet? _selectedPet;
    private List<Pet> _pets = [];

    public ObservableCollection<string> TaskTypes { get; } =
    [
        "Feeding", "Grooming", "Vet Appointment", "Exercise", "Other"
    ];

    public ObservableCollection<string> RecurrenceTypes { get; } =
    [
        "Daily", "Weekly", "Monthly"
    ];

    private string? _selectedTaskType;
    public string? SelectedTaskType
    {
        get => _selectedTaskType;
        set
        {
            SetProperty(ref _selectedTaskType, value);
            NotifyCanSaveChanged();
        }
    }

    private DateTime _taskDate = DateTime.Today;
    public DateTime TaskDate
    {
        get => _taskDate;
        set => SetProperty(ref _taskDate, value);
    }

    private TimeSpan _taskTime = DateTime.Now.TimeOfDay;
    public TimeSpan TaskTime
    {
        get => _taskTime;
        set => SetProperty(ref _taskTime, value);
    }

    private string? _notes;
    public string? Notes
    {
        get => _notes;
        set => SetProperty(ref _notes, value);
    }

    private bool _isCompleted = false;
    public bool IsCompleted
    {
        get => _isCompleted;
        set => SetProperty(ref _isCompleted, value);
    }

    public Pet? SelectedPet
    {
        get => _selectedPet;
        set
        {
            SetProperty(ref _selectedPet, value);
            SelectedPetId = value?.Id ?? 0;
            NotifyCanSaveChanged();
        }
    }

    public int SelectedPetId { get; private set; }

    public List<Pet> Pets
    {
        get => _pets;
        set => SetProperty(ref _pets, value);
    }

    private bool _isRecurring = false;
    public bool IsRecurring
    {
        get => _isRecurring;
        set => SetProperty(ref _isRecurring, value);
    }

    private string _recurrenceType = string.Empty;
    public string RecurrenceType
    {
        get => _recurrenceType;
        set => SetProperty(ref _recurrenceType, value);
    }

    private int _recurrenceInterval = 1;
    public int RecurrenceInterval
    {
        get => _recurrenceInterval;
        set => SetProperty(ref _recurrenceInterval, value);
    }

    private DateTime _endDate = DateTime.Today.AddMonths(1);
    public DateTime EndDate
    {
        get => _endDate;
        set => SetProperty(ref _endDate, value);
    }

    public ICommand SaveTaskCommand { get; }

    public bool CanSave => !string.IsNullOrWhiteSpace(SelectedTaskType) && SelectedPetId != 0;

    public TaskPageViewModel(DatabaseService databaseService, Tasks? existingTask = null)
    {
        _databaseService = databaseService;
        _isEditMode = existingTask != null;

        if (_isEditMode && existingTask != null)
        {
            SelectedTaskType = existingTask.Title;
            TaskDate = existingTask.ScheduledDate.Date;
            TaskTime = existingTask.ScheduledDate.TimeOfDay;
            Notes = existingTask.Notes;
            IsCompleted = existingTask.IsCompleted;
            SelectedPetId = existingTask.PetId;

            IsRecurring = existingTask.IsRecurring;
            RecurrenceType = existingTask.RecurrenceType;
            RecurrenceInterval = existingTask.RecurrenceInterval;
            EndDate = existingTask.EndDate;
        }

        SaveTaskCommand = new Command(async () => await SaveTask(), () => CanSave);
        PropertyChanged += (_, __) => ((Command)SaveTaskCommand).ChangeCanExecute();
    }

    private async Task SaveTask()
    {
        var scheduledDateTime = TaskDate.Add(TaskTime);

        if (scheduledDateTime < DateTime.Now)
        {
            Console.WriteLine("Scheduled date cannot be in the past.");
            return;
        }

        if (EndDate < DateTime.Now)
        {
            Console.WriteLine("End date cannot be in the past.");
            return;
        }

        if (IsRecurring && RecurrenceInterval <= 0)
        {
            Console.WriteLine("Recurrence interval must be a positive number.");
            return;
        }

        var task = new Tasks
        {
            Title = SelectedTaskType ?? "Task",
            ScheduledDate = scheduledDateTime,
            Notes = Notes,
            IsCompleted = false,
            PetId = SelectedPetId,
            RecurrenceType = RecurrenceType,
            RecurrenceInterval = RecurrenceInterval,
            EndDate = EndDate
        };

        if (IsRecurring)
        {
            await NotificationService.ScheduleRecurringNotifications(task);
        }
        else
        {
            await NotificationService.ScheduleNotificationAsync(task);
        }

        if (_isEditMode)
        {
            await _databaseService.UpdateTaskAsync(task);
        }
        else
        {
            await _databaseService.InsertTasksAsync(task);
        }

        await Shell.Current.GoToAsync("..");
    }

    private void NotifyCanSaveChanged()
    {
        OnPropertyChanged(nameof(CanSave));
        ((Command)SaveTaskCommand).ChangeCanExecute();
    }
}
