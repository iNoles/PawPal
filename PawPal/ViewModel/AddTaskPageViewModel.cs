using System.Collections.ObjectModel;
using System.Windows.Input;
using PawPal.Models;

namespace PawPal.ViewModel;

public class AddTaskPageViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    private ObservableCollection<Pet> _pets = [];
    public ObservableCollection<Pet> Pets
    {
        get => _pets;
        set => SetProperty(ref _pets, value);
    }

    private Pet? _selectedPet;
    public Pet? SelectedPet
    {
        get => _selectedPet;
        set => SetProperty(ref _selectedPet, value);
    }

    private string? _newTaskName;
    public string? NewTaskName
    {
        get => _newTaskName;
        set => SetProperty(ref _newTaskName, value);
    }

    private DateTime _newTaskDueDate = DateTime.Now;
    public DateTime NewTaskDueDate
    {
        get => _newTaskDueDate;
        set => SetProperty(ref _newTaskDueDate, value);
    }

    private string _newTaskCategory = "General";
    public string NewTaskCategory
    {
        get => _newTaskCategory;
        set => SetProperty(ref _newTaskCategory, value);
    }

    public ICommand SaveTaskCommand { get; }

    public AddTaskPageViewModel()
    {
        _databaseService = new DatabaseService();
        Pets = [.. _databaseService.GetPets()];

        SaveTaskCommand = new Command(async () => await SaveTask());
    }

    private async Task SaveTask()
    {
        if (string.IsNullOrWhiteSpace(NewTaskName))
        {
            App.Current?.Windows[0].Page?.DisplayAlert("Error", "Task Name cannot be empty!", "OK");
            return;
        }

        if (SelectedPet == null)
        {
            App.Current?.Windows[0].Page?.DisplayAlert("Error", "Please select a pet!", "OK");
            return;
        }

        var newTask = new Tasks
        {
            TaskName = NewTaskName,
            DueDate = NewTaskDueDate,
            PetId = SelectedPet.Id
        };
        _databaseService.InsertTasks(newTask);

        await NotificationService.ScheduleNotificationAsync(newTask);

        App.Current?.Windows[0].Page?.DisplayAlert("Success", "Task saved successfully!", "OK");


        // Reset fields or navigate back
        NewTaskName = string.Empty;
        NewTaskDueDate = DateTime.Now;
        NewTaskCategory = "General";
    }
}
