using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PawPal.Data;
using PawPal.Models;

namespace PawPal.ViewModel;

public class EditProfileViewModel : BaseViewModel
{
    private readonly AppDataContext _context;
    public Pet? SelectedPet { get; private set; }

    public ICommand SaveCommand { get; }

    public EditProfileViewModel(AppDataContext context)
    {
        _context = context;
        SaveCommand = new Command(async () => await SaveAsync());
    }

    public async Task InitializeAsync(int petId)
    {
        SelectedPet = await _context.Pets.FirstOrDefaultAsync(p => p.Id == petId);
        OnPropertyChanged(nameof(SelectedPet)); // Notify UI that SelectedPet is updated
    }

    private async Task SaveAsync()
    {
        if (SelectedPet != null)
        {
            // EF Core automatically tracks changes, so no need to call Update
            await _context.SaveChangesAsync();
            await Shell.Current.GoToAsync(".."); // Navigate back
        }
    }
}
