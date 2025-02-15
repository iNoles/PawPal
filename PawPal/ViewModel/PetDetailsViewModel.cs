using System.Collections.ObjectModel;
using System.Windows.Input;
using PawPal.Models;
using PawPal.Services;
using PawPal.Views;

namespace PawPal.ViewModel;

[QueryProperty(nameof(SelectedPet), nameof(SelectedPet))]
public class PetDetailsViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    private Pet? _selectedPet;
    public Pet? SelectedPet
    {
        get => _selectedPet;
        set
        {
            if (SetProperty(ref _selectedPet, value) && _selectedPet != null)
            {
                _ = LoadUpcomingTasks(); // Fire and forget to avoid blocking UI
            }
        }
    }


    private ObservableCollection<Tasks> _upcomingTasks = [];
    public ObservableCollection<Tasks> UpcomingTasks
    {
        get => _upcomingTasks;
        set => SetProperty(ref _upcomingTasks, value);
    }

    // Commands
    public ICommand EditProfileCommand { get; }
    public ICommand ViewMedicalRecordsCommand { get; }

    // Constructor
    public PetDetailsViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;

        // Initialize commands
        EditProfileCommand = new Command(async () => await EditProfile());
        ViewMedicalRecordsCommand = new Command(async () => await ViewMedicalRecords());
    }

    private async Task LoadUpcomingTasks()
    {
        if (SelectedPet == null)
            return;

        try
        {
            var tasks = await _databaseService.GetTasksForPetAsync(SelectedPet.Id);
            var filteredTasks = tasks.Where(t => !t.IsCompleted).OrderBy(t => t.ScheduledDate);
            UpcomingTasks = [.. filteredTasks];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading upcoming tasks: {ex.Message}");
        }
    }


    private async Task EditProfile()
    {
        if (SelectedPet == null)
            return;

        // Navigate to the Edit Profile page
        await Shell.Current.GoToAsync($"{nameof(TaskPage)}?editprofile?id={SelectedPet.Id}");
    }

    private async Task ViewMedicalRecords()
    {
        if (SelectedPet == null)
            return;

        // Navigate to the Medical Records page
        await Shell.Current.GoToAsync($"{nameof(MedicalRecordsPage)}?id={SelectedPet.Id}");

    }
}
