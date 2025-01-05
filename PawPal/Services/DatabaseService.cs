using PawPal.Models;
using SQLite;

namespace PawPal.Services;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService()
    {
        // Get the database file path
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "appdata.db");

        // Create the SQLite connection
        _database = new SQLiteAsyncConnection(databasePath);

        // Enable foreign key constraints
        _database.ExecuteAsync("PRAGMA foreign_keys = ON;");

        // Create multiple tables in a single call
        _database.CreateTablesAsync<Pet, Tasks, MedicalRecord, VetContact>();
    }

    // ==============================
    // PET MANAGEMENT
    // ==============================
    public Task<List<Pet>> GetAllPetsAsync()
    {
        return _database.Table<Pet>().ToListAsync();
    }
    public Task<Pet> GetPetByIdAsync(int id)
    {
        // Fetch the pet by ID
        return _database.Table<Pet>().FirstOrDefaultAsync(p => p.Id == id);
    }
    public Task<int> InsertPetAsync(Pet pet)
    {
       return _database.InsertAsync(pet);
    }
    public Task<int> UpdatePetAsync(Pet pet)
    {
        // Directly update the pet in the database if it exists
        return _database.UpdateAsync(pet);
    }

    // ==============================
    // TASK MANAGEMENT
    // ==============================
    public Task<List<Tasks>> GetTasksForPetAsync(int petId)
    {
        return _database.Table<Tasks>().Where(t => t.PetId == petId).ToListAsync();
    }
    public Task<List<Tasks>> GetTasksForMonthAsync(DateTime month)
    {
        var startOfMonth = new DateTime(month.Year, month.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

        return _database.Table<Tasks>().Where(task => task.ScheduledDate >= startOfMonth && task.ScheduledDate <= endOfMonth).ToListAsync();
    }
    public Task<int> InsertTasksAsync(Tasks tasks)
    {
        return _database.InsertAsync(tasks);
    }
    public Task<int> UpdateTaskAsync(Tasks task)
    {
        return _database.UpdateAsync(task);
    }

    // ==============================
    // MEDICAL RECORDS MANAGEMENT
    // ==============================
    public Task<List<MedicalRecord>> GetMedicalRecordsForPetAsync(int petId)
    {
        return _database.Table<MedicalRecord>().Where(r => r.PetId == petId).ToListAsync();
    }
    public Task<int> InsertMedicalRecordAsync(MedicalRecord record)
    {
        return _database.InsertAsync(record);
    }
    public Task<int> UpdateMedicalRecordAsync(MedicalRecord record)
    {
       return _database.UpdateAsync(record);
    }
    public Task<int> DeleteMedicalRecordAsync(MedicalRecord record)
    {
        return _database.DeleteAsync(record);
    }

    // ==============================
    // VET CONTACTS MANAGEMENT
    // ==============================
    public Task<List<VetContact>> GetAllVetContactsAsync() {
        return _database.Table<VetContact>().ToListAsync();
    }
    public Task<int> InsertVetContactAsync(VetContact contact) {
        return _database.InsertAsync(contact);
    }
    public Task<int> UpdateVetContactAsync(VetContact contact) {
        return _database.UpdateAsync(contact);
    }
    public Task<int> DeleteVetContact(VetContact contact) {
        return _database.DeleteAsync(contact);
    }
}
