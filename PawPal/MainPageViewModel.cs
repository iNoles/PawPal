using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PawPal;

public class MainPageViewModel : INotifyPropertyChanged
{
    private readonly PetRepository _petRepository;

    public ObservableCollection<Pet> Pets { get; } = [];

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

    public ICommand AddPetCommand { get; }
    public ICommand LoadPetsCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public MainPageViewModel(PetRepository petRepository)
    {
        _petRepository = petRepository;

        AddPetCommand = new Command(async () => await AddPetAsync());
        LoadPetsCommand = new Command(async () => await LoadPetsAsync());
    }

    private async Task AddPetAsync()
    {
        if (string.IsNullOrWhiteSpace(NewPetName) || string.IsNullOrWhiteSpace(NewPetSpecies))
        {
            // Handle validation error (e.g., show an alert)
            return; // Return a completed task
        }

        var newPet = new Pet
        {
            Name = NewPetName,
            Species = NewPetSpecies,
            DateOfBirth = NewPetDateOfBirth
        };

        await _petRepository.AddPetAsync(newPet);
        await LoadPetsAsync();
        ClearNewPetFields();
    }

    private async Task LoadPetsAsync()
    {
        Pets.Clear();
        var pets = await _petRepository.GetPetsWithTasksAsync();
        foreach (var pet in pets)
        {
            Pets.Add(pet);
        }
    }

    private void ClearNewPetFields()
    {
        NewPetName = string.Empty;
        NewPetSpecies = string.Empty;
        NewPetDateOfBirth = DateTime.Now;
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
