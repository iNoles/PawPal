using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PawPal.Data;
using PawPal.Models;

namespace PawPal.ViewModel;

public class AddVetContactViewModel : BaseViewModel
{
    private readonly AppDataContext _context;
    private readonly bool _isEditMode;
    private readonly VetContact? _existingVetContact;

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

    public AddVetContactViewModel(AppDataContext context, VetContact? existingVetContact = null)
    {
        _context = context;
        _isEditMode = existingVetContact != null;
        _existingVetContact = existingVetContact;

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
        try
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
                // Fetch the existing VetContact to ensure EF tracks it correctly
                var existingContact = await _context.VetContacts
                    .FirstOrDefaultAsync(v => v.Id == (_existingVetContact != null ? _existingVetContact.Id : 0));
                if (existingContact != null)
                {
                    // Update existing contact fields
                    existingContact.Name = Name;
                    existingContact.PhoneNumber = Phone;
                    existingContact.Email = Email;
                    existingContact.Address = Address;
                    existingContact.Notes = Notes;
                }
            }
            else
            {
                _context.VetContacts.Add(contact); // Add new contact
            }

            await _context.SaveChangesAsync();
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            // Handle exception (e.g., log it or display a message to the user)
            Console.WriteLine(ex.Message);
        }
    }
}
