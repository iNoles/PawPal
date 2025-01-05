using System.Windows.Input;
using PawPal.Models;
using PawPal.Services;

namespace PawPal.ViewModel;

public class AddVetContactViewModel: BaseViewModel
{
    private readonly DatabaseService _databaseService;
    private readonly bool _isEditMode;

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string _phone = string.Empty;
    public string Phone
    {
        get => _phone;
        set => SetProperty(ref _phone, value);
    }

    private string? _email;
    public string? Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    private string? _address;
    public string? Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    private string? _notes;
    public string? Notes
    {
        get => _notes;
        set => SetProperty(ref _notes, value);
    }

    public ICommand SaveTaskCommand { get; }

    public AddVetContactViewModel(DatabaseService databaseService, VetContact? existingVetContact = null)
    {
        _databaseService = databaseService;
        _isEditMode = existingVetContact != null;

        if (existingVetContact != null)
        {
            // Populate fields with the existing vet contact details for editing
            Name = existingVetContact.Name;
            Phone = existingVetContact.PhoneNumber;
            Email = existingVetContact.Email;
            Address = existingVetContact.Address;
            Notes = existingVetContact.Notes;
        }

        SaveTaskCommand = new Command(async () => await SaveVetContact());
    }

    private async Task SaveVetContact() {
        var contact = new VetContact
        {
            Name = Name,
            PhoneNumber = Phone,
            Email =  Email,
            Address = Address,
            Notes = Notes
        };

        if (_isEditMode)
        {
            await _databaseService.UpdateVetContactAsync(contact);
        }
        else
        {
            await _databaseService.InsertVetContactAsync(contact);
        }

        // Navigate back to the previous page
        await Shell.Current.GoToAsync("..");
    }
}
