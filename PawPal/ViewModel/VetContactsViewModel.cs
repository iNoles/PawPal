using System.Collections.ObjectModel;
using PawPal.Models;
using PawPal.Services;
using PawPal.Views;

namespace PawPal.ViewModel;

public class VetContactsViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    private ObservableCollection<VetContact> _vetContacts = [];
    public ObservableCollection<VetContact> VetContacts
    {
        get => _vetContacts;
        set => SetProperty(ref _vetContacts, value);
    }

    private VetContact? _selectedVetContact;
    public VetContact? SelectedVetContact
    {
        get => _selectedVetContact;
        set
        {
            if (SetProperty(ref _selectedVetContact, value) && value != null)
            {
                OnVetContactSelected(value);
            }
        }
    }

    public Command AddVetContactCommand { get; }
    public Command LoadVetContactsCommand { get; }

    public VetContactsViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;

        AddVetContactCommand = new Command(OnAddVetContact);
        LoadVetContactsCommand = new Command(async () => await LoadVetContacts());

        _ = LoadVetContacts(); // Fire and forget, stays on main thread
    }

    private async Task LoadVetContacts()
    {
        var contacts = await _databaseService.GetAllVetContactsAsync();
        VetContacts = [.. contacts];
        OnPropertyChanged(nameof(VetContacts)); // Ensure UI updates
    }

    private async void OnAddVetContact()
    {
        await Shell.Current.GoToAsync(nameof(AddEditVetContactPage));
    }

    private async void OnVetContactSelected(VetContact vetContact)
    {
        var parameters = new Dictionary<string, object> { { "VetContact", vetContact } };
        await Shell.Current.GoToAsync(nameof(AddEditVetContactPage), parameters);
    }
}
