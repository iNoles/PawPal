using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PawPal;

public class AddTaskPageViewModel : INotifyPropertyChanged
{
    private readonly DatabaseService _databaseService;
    private ObservableCollection<Pet> _pets = [];
    private Pet? _selectedPet;
    private string _newTaskName = string.Empty;
    private DateTime _newTaskDueDate = DateTime.Now;

    public ObservableCollection<Pet> Pets
    {
        get => _pets;
        set => SetProperty(ref _pets, value);
    }

    public Pet? SelectedPet
    {
        get => _selectedPet;
        set
        {
            SetProperty(ref _selectedPet, value);
            UpdateCommandStates();
        }
    }

    public string NewTaskName
    {
        get => _newTaskName;
        set
        {
            SetProperty(ref _newTaskName, value);
            UpdateCommandStates();
        }
    }

    public DateTime NewTaskDueDate
    {
        get => _newTaskDueDate;
        set => SetProperty(ref _newTaskDueDate, value);
    }

    public ICommand AddTaskCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public AddTaskPageViewModel()
    {
        _databaseService = new DatabaseService();
        Pets = [.. _databaseService.GetPets()];

        AddTaskCommand = new Command(
            async () => await AddTasksAsync(),
            () => SelectedPet != null && !string.IsNullOrWhiteSpace(NewTaskName)
        );
    }

    private async Task AddTasksAsync()
    {
        try
        {
            if (SelectedPet == null || string.IsNullOrWhiteSpace(NewTaskName))
                return;

            var newTask = new Tasks
            {
                TaskName = NewTaskName,
                DueDate = NewTaskDueDate,
                PetId = SelectedPet.Id
            };

            _databaseService.InsertTasks(newTask);

            await NotificationService.ScheduleNotificationAsync(newTask);

            NewTaskName = string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding task: {ex.Message}");
        }
    }

    private void UpdateCommandStates()
    {
        ((Command)AddTaskCommand).ChangeCanExecute();
    }

    protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (!Equals(field, value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
