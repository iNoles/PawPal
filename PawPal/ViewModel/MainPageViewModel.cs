using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PawPal.Data;
using PawPal.Models;

namespace PawPal.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly AppDataContext _context;

        private Pet? _selectedPet;

        private string _newPetName = string.Empty;
        public string NewPetName
        {
            get => _newPetName;
            set => SetProperty(ref _newPetName, value);
        }

        private string _newPetSpecies = string.Empty;
        public string NewPetSpecies
        {
            get => _newPetSpecies;
            set => SetProperty(ref _newPetSpecies, value);
        }

        private DateTime _newPetDateOfBirth = DateTime.Now;
        public DateTime NewPetDateOfBirth
        {
            get => _newPetDateOfBirth;
            set => SetProperty(ref _newPetDateOfBirth, value);
        }

        public ObservableCollection<Pet> Pets { get; } = [];
        public ObservableCollection<PetTask> UpcomingTasks { get; } = [];

        public Pet? SelectedPet
        {
            get => _selectedPet;
            set
            {
                if (SetProperty(ref _selectedPet, value))
                {
                    if (_selectedPet != null)
                    {
                        _ = LoadTasksForSelectedPet(_selectedPet.Id);
                    }
                    else
                    {
                        UpcomingTasks.Clear(); // Clear tasks when no pet is selected
                    }
                }
            }
        }

        public ICommand InsertPetCommand { get; }

        public MainPageViewModel(AppDataContext context)
        {
            _context = context;
            InsertPetCommand = new Command(async () => await InsertPet(), () => CanInsertPet);
        }

        public bool CanInsertPet => !string.IsNullOrWhiteSpace(NewPetName) && !string.IsNullOrWhiteSpace(NewPetSpecies);

        public async Task InitializeAsync()
        {
            await LoadPets();
        }

        private async Task LoadPets()
        {
            var pets = await _context.Pets.ToListAsync();
            Pets.Clear();
            foreach (var pet in pets)
            {
                Pets.Add(pet);
            }
        }

        private async Task LoadTasksForSelectedPet(int petId)
        {
            var tasks = await _context.PetTasks.Where(t => t.PetId == petId).ToListAsync();
            var filteredTasks = tasks.Where(t => !t.IsCompleted).OrderBy(t => t.ScheduledDate);

            UpcomingTasks.Clear();
            foreach (var task in filteredTasks)
            {
                UpcomingTasks.Add(task);
            }
        }

        private async Task InsertPet()
        {
            if (string.IsNullOrWhiteSpace(NewPetName) || string.IsNullOrWhiteSpace(NewPetSpecies))
                return;

            var newPet = new Pet
            {
                Name = NewPetName,
                Species = NewPetSpecies,
                DateOfBirth = NewPetDateOfBirth
            };

            await _context.Pets.AddAsync(newPet); // Add the pet to the DbSet
            await _context.SaveChangesAsync(); // Ensure pet is saved before updating the UI collection
            Pets.Add(newPet); // Add to the ObservableCollection after saving
            ClearNewPetFields();
        }

        private void ClearNewPetFields()
        {
            NewPetName = string.Empty;
            NewPetSpecies = string.Empty;
            NewPetDateOfBirth = DateTime.Now;
        }
    }
}
