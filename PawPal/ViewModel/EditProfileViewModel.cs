using System.Windows.Input;
using PawPal.Models;
using PawPal.Services;

namespace PawPal.ViewModel;

public class EditProfileViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;
    public Pet SelectedPet { get; private set; }

    public ICommand SaveCommand { get; }

    public EditProfileViewModel(DatabaseService databaseService, int petId)
    {
        _databaseService = databaseService;
        LoadPets(petId);

        SaveCommand = new Command(Save);
    }

    private async void LoadPets(int petId)
    {
        SelectedPet = await _databaseService.GetPetByIdAsync(petId);
    }

    private async void Save()
    {
        await _databaseService.UpdatePetAsync(SelectedPet);
        await Shell.Current.GoToAsync(".."); // Navigate back to the previous page
    }
}

