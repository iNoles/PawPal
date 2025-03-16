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
    public Command CallVetCommand { get; }
    public Command EmailVetCommand { get; }
    public Command NavigateToVetCommand { get; }

    public VetContactsViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;

        AddVetContactCommand = new Command(OnAddVetContact);
        LoadVetContactsCommand = new Command(async () => await LoadVetContacts());
        CallVetCommand = new Command<VetContact>(async (vet) => await CallVet(vet));
        EmailVetCommand = new Command<VetContact>(async (vet) => await EmailVet(vet));
        NavigateToVetCommand = new Command<VetContact>(async (vet) => await NavigateToVet(vet));

        _ = LoadVetContacts(); // Fire and forget, stays on main thread
    }

    private async Task LoadVetContacts()
    {
        var contacts = await _databaseService.GetAllVetContactsAsync();
        VetContacts = [.. contacts.OrderByDescending(c => c.IsEmergency)];
        OnPropertyChanged(nameof(VetContacts)); 
    }

    private static async void OnAddVetContact()
    {
        await Shell.Current.GoToAsync(nameof(AddEditVetContactPage));
    }

    private static async void OnVetContactSelected(VetContact vetContact)
    {
        var parameters = new Dictionary<string, object> { { "VetContact", vetContact } };
        await Shell.Current.GoToAsync(nameof(AddEditVetContactPage), parameters);
    }

    private static async Task CallVet(VetContact vet)
    {
        if (!string.IsNullOrWhiteSpace(vet.PhoneNumber))
        {
            await Launcher.OpenAsync(new Uri($"tel:{vet.PhoneNumber}"));
        }
    }
    
    private static async Task EmailVet(VetContact vet)
    {
        if (!string.IsNullOrWhiteSpace(vet.Email))
        {
            await Launcher.OpenAsync(new Uri($"mailto:{vet.Email}"));
        }
    }
    
    private static async Task NavigateToVet(VetContact vet)
    {
        if (!string.IsNullOrWhiteSpace(vet.Address))
        {
            await Launcher.OpenAsync(new Uri($"https://maps.google.com/?q={vet.Address}"));
        }
    }
}
