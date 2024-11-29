using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PawPal;

public class MainPageViewModel : INotifyPropertyChanged
{
    private readonly DatabaseService _databaseService;
    private ObservableCollection<Pet> _pets = [];
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
            _selectedPet = value;
            if (_selectedPet != null)
            {
                NavigateToProfile(_selectedPet);
            }
        }
    }

    // Command for inserting a new pet
    public ICommand InsertPetCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public MainPageViewModel()
    {
        _databaseService = new DatabaseService();
        Pets = [.. _databaseService.GetPets()];

        // Initialize the InsertPetCommand with the method that handles the pet insertion
        InsertPetCommand = new Command(InsertPet);
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

    private async static void NavigateToProfile(Pet pet)
    {
        Dictionary<string, object> parameters = new() {
            { "id", pet.Id },
            { "name", pet.Name },
            { "species", pet.Species},
            { "breed", pet.Breed ?? string.Empty},
            { "birthDate", pet.DateOfBirth},
            { "medical", pet.MedicalRecords ?? string.Empty}
        };
        await Shell.Current.GoToAsync("profile", parameters);
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
