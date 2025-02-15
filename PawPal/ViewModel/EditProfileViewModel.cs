using System.Windows.Input;
using PawPal.Models;
using PawPal.Services;

namespace PawPal.ViewModel;

public class EditProfileViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;
    public Pet? SelectedPet { get; private set; }

    public ICommand SaveCommand { get; }

    public EditProfileViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        SaveCommand = new Command(async () => await SaveAsync());
    }

    public async Task InitializeAsync(int petId)
    {
        SelectedPet = await _databaseService.GetPetByIdAsync(petId);
        OnPropertyChanged(nameof(SelectedPet)); // Notify UI that SelectedPet is updated
    }

    private async Task SaveAsync()
    {
        if (SelectedPet != null)
        {
            await _databaseService.UpdatePetAsync(SelectedPet);
            await Shell.Current.GoToAsync(".."); // Navigate back
        }
    }
}
