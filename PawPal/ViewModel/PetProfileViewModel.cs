namespace PawPal.ViewModel;

public class PetProfileViewModel : BaseViewModel
{
    private Pet? _selectedPet;

    public Pet? SelectedPet
    {
        get => _selectedPet;
        set
        {
            _selectedPet = value;
            OnPropertyChanged();
        }
    }
}
