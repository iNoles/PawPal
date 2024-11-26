using Microsoft.EntityFrameworkCore;

namespace PawPal;

public class PetRepository(PetCareDbContext dbContext)
{
    private readonly PetCareDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    // Add a new pet
    public async Task<Pet> AddPetAsync(Pet pet)
    {
        ArgumentNullException.ThrowIfNull(pet);
        if (_dbContext.Pets != null)
        {
            await _dbContext.Pets.AddAsync(pet);
            await _dbContext.SaveChangesAsync();
        }
        return pet;
    }

    // Add a new task to a pet
    public async Task AddTaskToPetAsync(int petId, Tasks task)
    {
        ArgumentNullException.ThrowIfNull(task);

        if (_dbContext.Pets == null)
            throw new InvalidOperationException("Pets DbSet is null.");

        var pet = await _dbContext.Pets
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == petId) ?? throw new InvalidOperationException($"Pet with ID {petId} not found.");

        pet.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();
    }

    // Get all pets with their tasks
    public async Task<List<Pet>> GetPetsWithTasksAsync()
    {
        if (_dbContext.Pets == null) return []; // Handle null DbSet

        return await _dbContext.Pets.Include(p => p.Tasks).ToListAsync();
    }

    // Get a specific pet with tasks
    public async Task<Pet?> GetPetWithTasksAsync(int id)
    {
        if (_dbContext.Pets == null) return null; // Handle null DbSet

        return await _dbContext.Pets.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == id);
    }
}
