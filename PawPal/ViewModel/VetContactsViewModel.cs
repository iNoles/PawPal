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
                // Handle selection logic (e.g., navigate to detail page)
            }
        }
    }

    public Command AddVetContactCommand { get; }

    public VetContactsViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        LoadVetContacts();

        AddVetContactCommand = new Command(OnAddVetContact);
    }

    private async void LoadVetContacts()
    {
        VetContacts = [.. await _databaseService.GetAllVetContactsAsync()];
    }

    private async void OnAddVetContact()
    {
        // Navigate to AddEditVetContactPage
        await Shell.Current.GoToAsync(nameof(AddEditVetContactPage));
    }
}
