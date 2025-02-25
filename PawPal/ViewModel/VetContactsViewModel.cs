using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public Command<string> CallVetCommand { get; }
    public Command<string> EmailVetCommand { get; }

    public VetContactsViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;

        AddVetContactCommand = new Command(OnAddVetContact);
        LoadVetContactsCommand = new Command(async () => await LoadVetContacts());

        CallVetCommand = new Command<string>(OnCallVet);
        EmailVetCommand = new Command<string>(OnEmailVet);

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

    private void OnCallVet(string phoneNumber)
    {
        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            try
            {
                Launcher.OpenAsync($"tel:{phoneNumber}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to make a call: {ex.Message}");
            }
        }
    }

    private void OnEmailVet(string email)
    {
        if (!string.IsNullOrWhiteSpace(email))
        {
            try
            {
                Launcher.OpenAsync($"mailto:{email}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to open email client: {ex.Message}");
            }
        }

    }
}
