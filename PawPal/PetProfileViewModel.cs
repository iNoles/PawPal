using System.ComponentModel;

namespace PawPal;

public class PetProfileViewModel : INotifyPropertyChanged
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

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
