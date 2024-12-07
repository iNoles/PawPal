using System.Collections.ObjectModel;
using System.Windows.Input;
using PawPal.Models;

namespace PawPal.ViewModel;

public class MainPageViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    private Pet? _selectedPet;

    private string _newPetName = string.Empty;
    public string NewPetName
    {
        get => _newPetName;
        set => SetProperty(ref _newPetName, value);
    }

    private string _newPetSpecies = string.Empty;
    public string NewPetSpecies
    {
        get => _newPetSpecies;
        set => SetProperty(ref _newPetSpecies, value);
    }

    private DateTime _newPetDateOfBirth = DateTime.Now;
    public DateTime NewPetDateOfBirth
    {
        get => _newPetDateOfBirth;
        set => SetProperty(ref _newPetDateOfBirth, value);
    }

    private ObservableCollection<Pet> _pets = [];
    public ObservableCollection<Pet> Pets
    {
        get => _pets;
        set => SetProperty(ref _pets, value);
    }

    private ObservableCollection<Tasks> _upcomingTasks = [];
    public ObservableCollection<Tasks> UpcomingTasks
    {
        get => _upcomingTasks;
        set => SetProperty(ref _upcomingTasks, value);
    }

    public Pet? SelectedPet
    {
        get => _selectedPet;
        set
        {
            SetProperty(ref _selectedPet, value);
            if (_selectedPet != null)
            {
                LoadTasksForSelectedPet(_selectedPet.Id);
            }
        }
    }

    // Command for inserting a new pet
    public ICommand InsertPetCommand { get; }
    public ICommand ViewDetailsCommand { get; }

    public MainPageViewModel()
    {
        _databaseService = new DatabaseService();
        Pets = [.. _databaseService.GetPets()];

        // Initialize the InsertPetCommand with the method that handles the pet insertion
        InsertPetCommand = new Command(InsertPet);
        ViewDetailsCommand = new Command<Pet>(ViewDetails);
    }

    private void LoadTasksForSelectedPet(int petId)
    {
        // Fetch tasks for the selected pet
        var tasks = _databaseService.GetTasksForPet(petId);

        // Filter tasks that are not completed and sort by due date
        UpcomingTasks = [.. tasks.Where(t => !t.IsCompleted).OrderBy(t => t.ScheduledDate)];
    }

    private void InsertPet()
    {
        var newPet = new Pet
        {
            Name = NewPetName,
            Species = NewPetSpecies,
            DateOfBirth = NewPetDateOfBirth
        };

        _databaseService.InsertPet(newPet);
        Pets.Add(newPet);
        ClearNewPetFields();
    }

    private void ClearNewPetFields()
    {
        NewPetName = string.Empty;
        NewPetSpecies = string.Empty;
        NewPetDateOfBirth = DateTime.Now;
    }

    private async void ViewDetails(Pet pet)
    {
        // Navigate to the PetDetailsPage with the selected pet's ID
        if (pet != null)
        {
            await Shell.Current.GoToAsync($"petdetails", new Dictionary<string, object>
            {
                [nameof(Pet)] = pet
            });
        }
    }
}
