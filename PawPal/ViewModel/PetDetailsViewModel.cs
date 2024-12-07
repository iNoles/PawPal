using System.Collections.ObjectModel;
using System.Windows.Input;
using PawPal.Models;

namespace PawPal.ViewModel;

//TODO: Implement Medical Records
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
            if (SetProperty(ref _selectedPet, value))
            {
                // Load tasks when the SelectedPet changes
                LoadUpcomingTasks();
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
    public PetDetailsViewModel()
    {
        _databaseService = new DatabaseService();

        // Initialize commands
        EditProfileCommand = new Command(async () => await EditProfile());
        ViewMedicalRecordsCommand = new Command(async () => await ViewMedicalRecords());
    }

    private void LoadUpcomingTasks()
    {
        if (SelectedPet == null)
            return;

        // Fetch tasks for the selected pet
        var tasks = _databaseService.GetTasksForPet(SelectedPet.Id);

        // Filter upcoming tasks
        var filteredTasks = tasks.Where(t => !t.IsCompleted).OrderBy(t => t.ScheduledDate);
        UpcomingTasks = [.. filteredTasks];
    }

    private async Task EditProfile()
    {
        if (SelectedPet == null)
            return;

        // Navigate to the Edit Profile page
        await Shell.Current.GoToAsync($"editprofile?id={SelectedPet.Id}");
    }

    private async Task ViewMedicalRecords()
    {
        if (SelectedPet == null)
            return;

        // Navigate to the Medical Records page
        await Shell.Current.GoToAsync($"medicalrecords?id={SelectedPet.Id}");
    }
}
