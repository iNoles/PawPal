using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PawPal.Data;
using PawPal.Models;
using PawPal.Views;

namespace PawPal.ViewModel
{
    [QueryProperty(nameof(SelectedPet), nameof(SelectedPet))]
    public class PetDetailsViewModel : BaseViewModel
    {
        private readonly AppDataContext _context;

        private Pet? _selectedPet;
        public Pet? SelectedPet
        {
            get => _selectedPet;
            set
            {
                if (SetProperty(ref _selectedPet, value) && _selectedPet != null)
                {
                    _ = LoadUpcomingTasks(); // Fire-and-forget but will not block the UI
                }
            }
        }

        private ObservableCollection<PetTask> _upcomingTasks = [];
        public ObservableCollection<PetTask> UpcomingTasks
        {
            get => _upcomingTasks;
            set => SetProperty(ref _upcomingTasks, value);
        }

        // Commands
        public ICommand EditProfileCommand { get; }
        public ICommand ViewMedicalRecordsCommand { get; }

        // Constructor
        public PetDetailsViewModel(AppDataContext context)
        {
            _context = context;

            // Initialize commands
            EditProfileCommand = new Command(async () => await EditProfile());
            ViewMedicalRecordsCommand = new Command(async () => await ViewMedicalRecords());
        }

        // Async method to load upcoming tasks for the selected pet
        private async Task LoadUpcomingTasks()
        {
            if (SelectedPet == null)
                return;

            try
            {
                var tasks = await _context.PetTasks.Where(t => t.PetId == SelectedPet.Id).ToListAsync();
                var filteredTasks = tasks.Where(t => !t.IsCompleted).OrderBy(t => t.ScheduledDate);

                // Clear and add items to ObservableCollection
                UpcomingTasks.Clear();
                foreach (var task in filteredTasks)
                {
                    UpcomingTasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                // Log or handle error here more robustly
                Console.WriteLine($"Error loading upcoming tasks: {ex.Message}");
            }
        }

        // Command to edit pet profile
        private async Task EditProfile()
        {
            if (SelectedPet == null)
                return;

            // Navigate to the Edit Profile page
            await Shell.Current.GoToAsync($"{nameof(TaskPage)}?editprofile?id={SelectedPet.Id}");
        }

        // Command to view medical records for the selected pet
        private async Task ViewMedicalRecords()
        {
            if (SelectedPet == null)
                return;

            // Navigate to the Medical Records page
            await Shell.Current.GoToAsync($"{nameof(MedicalRecordsPage)}?id={SelectedPet.Id}");
        }
    }
}
