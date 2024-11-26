using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PawPal;

public class AddTaskPageViewModel : INotifyPropertyChanged
{
    private readonly PetRepository _petRepository;
    private List<Pet> _pets = [];
    private Pet? _selectedPet;
    private string _newTaskName = string.Empty;
    private DateTime _newTaskDueDate = DateTime.Now;

    public List<Pet> Pets
    {
        get => _pets;
        set => SetProperty(ref _pets, value);
    }

    public Pet? SelectedPet
    {
        get => _selectedPet;
        set => SetProperty(ref _selectedPet, value);
    }

    public string NewTaskName
    {
        get => _newTaskName;
        set => SetProperty(ref _newTaskName, value);
    }

    public DateTime NewTaskDueDate
    {
        get => _newTaskDueDate;
        set => SetProperty(ref _newTaskDueDate, value);
    }

    public ICommand AddTaskCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public AddTaskPageViewModel(PetRepository petRepository, NotificationService notificationService)
    {
        _petRepository = petRepository;
        AddTaskCommand = new Command(async () => await AddTasksAsync());
        LoadPetsAsync();
    }

    private async void LoadPetsAsync()
    {
        Pets = await _petRepository.GetPetsWithTasksAsync();
    }

    private async Task AddTasksAsync()
    {
        if (SelectedPet == null || string.IsNullOrWhiteSpace(NewTaskName))
            return;

        var newTask = new Tasks
        {
            TaskName = NewTaskName,
            DueDate = NewTaskDueDate,
            Pet = SelectedPet
        };

        // Save the task to the pet and repository
        await _petRepository.AddTaskToPetAsync(SelectedPet.Id, newTask);

        // Schedule a notification for the task
        NotificationService.SchedulePetActivityReminder(newTask.TaskName, newTask.DueDate, SelectedPet.Name);

        NewTaskName = string.Empty; // Clear the task input
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