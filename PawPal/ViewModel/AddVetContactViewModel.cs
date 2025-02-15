using System.Windows.Input;
using PawPal.Models;
using PawPal.Services;

namespace PawPal.ViewModel;

public class AddVetContactViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;
    private readonly bool _isEditMode;

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set
        {
            if (SetProperty(ref _name, value))
            {
                OnPropertyChanged(nameof(IsNameInvalid));
                OnPropertyChanged(nameof(CanSave));
            }
        }
    }
    public bool IsNameInvalid => string.IsNullOrWhiteSpace(Name);

    private string _phone = string.Empty;
    public string Phone
    {
        get => _phone;
        set
        {
            if (SetProperty(ref _phone, value))
            {
                OnPropertyChanged(nameof(IsPhoneInvalid));
                OnPropertyChanged(nameof(CanSave));
            }
        }
    }
    public bool IsPhoneInvalid => string.IsNullOrWhiteSpace(Phone);

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

    public bool CanSave => !IsNameInvalid && !IsPhoneInvalid;

    public ICommand SaveTaskCommand { get; }

    public AddVetContactViewModel(DatabaseService databaseService, VetContact? existingVetContact = null)
    {
        _databaseService = databaseService;
        _isEditMode = existingVetContact != null;

        if (existingVetContact != null)
        {
            Name = existingVetContact.Name;
            Phone = existingVetContact.PhoneNumber;
            Email = existingVetContact.Email;
            Address = existingVetContact.Address;
            Notes = existingVetContact.Notes;
        }

        SaveTaskCommand = new Command(async () => await SaveVetContact(), () => CanSave);
    }

    private async Task SaveVetContact()
    {
        var contact = new VetContact
        {
            Name = Name,
            PhoneNumber = Phone,
            Email = Email,
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

        await Shell.Current.GoToAsync("..");
    }
}
