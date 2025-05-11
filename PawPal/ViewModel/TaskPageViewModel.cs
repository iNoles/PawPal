namespace PawPal.ViewModel;

using System.Collections.ObjectModel;
using System.Windows.Input;
using PawPal.Data;
using PawPal.Models;
using PawPal.Services;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

public class TaskPageViewModel : BaseViewModel
{
    private readonly AppDataContext _context;
    private readonly bool _isEditMode;
    private Pet? _selectedPet;
    private List<Pet> _pets = [];

    // Task Types and Recurrence Types
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

    public bool IsBusy { get; private set; }

    public TaskPageViewModel(AppDataContext context, PetTask? existingTask = null)
    {
        _context = context;
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
        else
        {
            // Set default task type if creating new task
            SelectedTaskType = "Feeding"; // or any default task type
        }

        SaveTaskCommand = new Command(async () => await SaveTask(), () => CanSave);
        PropertyChanged += (_, __) => ((Command)SaveTaskCommand).ChangeCanExecute();
    }

    private async Task SaveTask()
    {
        IsBusy = true;  // Show loading indicator
        NotifyCanSaveChanged();

        var scheduledDateTime = TaskDate.Add(TaskTime);

        if (scheduledDateTime < DateTime.Now)
        {
            Console.WriteLine("Scheduled date cannot be in the past.");
            IsBusy = false;
            NotifyCanSaveChanged();
            return;
        }

        if (EndDate < DateTime.Now)
        {
            Console.WriteLine("End date cannot be in the past.");
            IsBusy = false;
            NotifyCanSaveChanged();
            return;
        }

        if (IsRecurring && RecurrenceInterval <= 0)
        {
            Console.WriteLine("Recurrence interval must be a positive number.");
            IsBusy = false;
            NotifyCanSaveChanged();
            return;
        }

        var task = new PetTask
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
            // Handle recurring task logic
            var currentTaskDate = scheduledDateTime;
            while (currentTaskDate <= EndDate)
            {
                await NotificationService.ScheduleNotificationAsync(task);

                // Calculate next occurrence based on the RecurrenceType
                currentTaskDate = RecurrenceType switch
                {
                    "Daily" => currentTaskDate.AddDays(RecurrenceInterval),
                    "Weekly" => currentTaskDate.AddDays(RecurrenceInterval * 7), // Multiply by 7 to add weeks
                    "Monthly" => currentTaskDate.AddMonths(RecurrenceInterval),
                    _ => throw new InvalidOperationException("Invalid recurrence type.")
                };
            }
        }
        else
        {
            await NotificationService.ScheduleNotificationAsync(task);
        }

        if (_isEditMode)
        {
            var existingTask = await _context.PetTasks.FirstOrDefaultAsync(t => t.Id == task.Id);
            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.ScheduledDate = task.ScheduledDate;
                existingTask.Notes = task.Notes;
                existingTask.IsCompleted = task.IsCompleted;
                existingTask.PetId = task.PetId;
                existingTask.RecurrenceType = task.RecurrenceType;
                existingTask.RecurrenceInterval = task.RecurrenceInterval;
                existingTask.EndDate = task.EndDate;

                await _context.SaveChangesAsync();
            }
        }
        else
        {
            await _context.PetTasks.AddAsync(task);
        }

        await _context.SaveChangesAsync();

        IsBusy = false;  // Hide loading indicator
        NotifyCanSaveChanged();

        await Shell.Current.GoToAsync("..");
    }

    private void NotifyCanSaveChanged()
    {
        OnPropertyChanged(nameof(CanSave));
        ((Command)SaveTaskCommand).ChangeCanExecute();
    }
}
