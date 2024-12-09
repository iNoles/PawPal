using System.Windows.Input;
using PawPal.Models;

namespace PawPal.ViewModel;

public class EditProfileViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;
    public Pet SelectedPet { get; private set; }

    public ICommand SaveCommand { get; }

    public EditProfileViewModel(int petId)
    {
        _databaseService = new DatabaseService();
        SelectedPet = _databaseService.GetPetById(petId);

        SaveCommand = new Command(Save);
    }

    private async void Save()
    {
        _databaseService.UpdatePet(SelectedPet);
        await Shell.Current.GoToAsync(".."); // Navigate back to the previous page
    }
}

