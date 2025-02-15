using System.Collections.ObjectModel;
using System.Windows.Input;
using PawPal.Models;
using PawPal.Services;

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

    public ObservableCollection<Pet> Pets { get; } = new();
    public ObservableCollection<Tasks> UpcomingTasks { get; } = new();

    public Pet? SelectedPet
    {
        get => _selectedPet;
        set
        {
            if (SetProperty(ref _selectedPet, value) && _selectedPet != null)
            {
                _ = LoadTasksForSelectedPet(_selectedPet.Id);
            }
        }
    }

    public ICommand InsertPetCommand { get; }

    public MainPageViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        InsertPetCommand = new Command(async () => await InsertPet());
    }

    public async Task InitializeAsync()
    {
        await LoadPets();
    }

    private async Task LoadPets()
    {
        var pets = await _databaseService.GetAllPetsAsync();
        Pets.Clear();
        foreach (var pet in pets)
        {
            Pets.Add(pet);
        }
    }

    private async Task LoadTasksForSelectedPet(int petId)
    {
        var tasks = await _databaseService.GetTasksForPetAsync(petId);
        var filteredTasks = tasks.Where(t => !t.IsCompleted).OrderBy(t => t.ScheduledDate);

        UpcomingTasks.Clear();
        foreach (var task in filteredTasks)
        {
            UpcomingTasks.Add(task);
        }
    }

    private async Task InsertPet()
    {
        if (string.IsNullOrWhiteSpace(NewPetName) || string.IsNullOrWhiteSpace(NewPetSpecies))
            return;

        var newPet = new Pet
        {
            Name = NewPetName,
            Species = NewPetSpecies,
            DateOfBirth = NewPetDateOfBirth
        };

        await _databaseService.InsertPetAsync(newPet);
        Pets.Add(newPet);
        ClearNewPetFields();
    }

    private void ClearNewPetFields()
    {
        NewPetName = string.Empty;
        NewPetSpecies = string.Empty;
        NewPetDateOfBirth = DateTime.Now;
    }
}
