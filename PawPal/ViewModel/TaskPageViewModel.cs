using System.Collections.ObjectModel;
using System.Windows.Input;
using PawPal.Models;
using PawPal.Services;

namespace PawPal.ViewModel;

public class TaskPageViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;
    private readonly bool _isEditMode;

    private int _selectedPetId;
    private List<Pet> _pets = [];

    public ObservableCollection<string> TaskTypes { get; } =
    [
        "Feeding", "Grooming", "Vet Appointment", "Exercise", "Other"
    ];

    // Task Properties
    private string? _selectedTaskType;
    public string? SelectedTaskType
    {
        get => _selectedTaskType;
        set => SetProperty(ref _selectedTaskType, value);
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

    public int SelectedPetId
    {
        get => _selectedPetId;
        set => SetProperty(ref _selectedPetId, value);
    }

    // List of pets (you should populate this with actual pets in your application)
    public List<Pet> Pets
    {
        get => _pets;
        set => SetProperty(ref _pets, value);
    }

    // Command for saving the task
    public ICommand SaveTaskCommand { get; }

    // Validation property to enable the Save button
    public bool CanSave => !string.IsNullOrWhiteSpace(SelectedTaskType) && SelectedPetId != 0;

    // Constructor
    public TaskPageViewModel(DatabaseService databaseService, Tasks? existingTask = null)
    {
        _databaseService = databaseService;
        _isEditMode = existingTask != null;

        if (existingTask != null)
        {
            // Populate fields with the existing task details for editing
            SelectedTaskType = existingTask.Title;
            TaskDate = existingTask.ScheduledDate.Date;
            TaskTime = existingTask.ScheduledDate.TimeOfDay;
            Notes = existingTask.Notes;
            IsCompleted = existingTask.IsCompleted;
            SelectedPetId = existingTask.PetId;
        }

        SaveTaskCommand = new Command(async () => await SaveTask(), () => CanSave);
        PropertyChanged += (_, __) => ((Command)SaveTaskCommand).ChangeCanExecute();
    }

    private async Task SaveTask()
    {
        // Combine date and time
        var scheduledDateTime = TaskDate.Add(TaskTime);

        var task = new Tasks
        {
            Title = SelectedTaskType ?? "Task",
            ScheduledDate = scheduledDateTime,
            Notes = Notes,
            IsCompleted = false,
            PetId = SelectedPetId
        };

        await NotificationService.ScheduleNotificationAsync(task);

        if (_isEditMode)
        {
            await _databaseService.UpdateTaskAsync(task);
        }
        else
        {
            await _databaseService.InsertTasksAsync(task);
        }

        // Navigate back to the previous page
        await Shell.Current.GoToAsync("..");
    }
}
