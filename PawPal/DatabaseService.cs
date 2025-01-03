using PawPal.Models;
using SQLite;

namespace PawPal;

public class DatabaseService
{
    private readonly SQLiteConnection _database;

    public DatabaseService()
    {
        // Get the database file path
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "appdata.db");

        // Create the SQLite connection
        _database = new SQLiteConnection(databasePath);

        // Enable foreign key constraints
        _database.Execute("PRAGMA foreign_keys = ON;");

        // Create multiple tables in a single call
        _database.CreateTables<Pet, Tasks, MedicalRecord>();
    }

    // Insert a new pet
    public void InsertPet(Pet pet)
    {
        _database.Insert(pet);
    }

    // Insert a new task
    public void InsertTasks(Tasks tasks)
    {
        _database.Insert(tasks);
    }

    // Insert a new medical record
    public void InsertMedicalRecord(MedicalRecord record)
    {
        _database.Insert(record);
    }

    // Update an existing pet
    public void UpdatePet(Pet pet)
    {
        // Directly update the pet in the database if it exists
        _database.Update(pet);
    }

    // Update an existing task
    public void UpdateTask(Tasks task)
    {
        _database.Update(task);
    }
    
    // Update an existing medical record
    public void UpdateMedicalRecord(MedicalRecord record)
    {
        _database.Update(record);
    }

    // Fetch all pets
    public List<Pet> GetPets()
    {
        return [.. _database.Table<Pet>()];
    }

    // Fetch tasks for a specific pet
    public List<Tasks> GetTasksForPet(int petId)
    {
        return [.. _database.Table<Tasks>().Where(t => t.PetId == petId)];
    }
    
    // Fetch tasks within a specific month
    public List<Tasks> GetTasksForMonth(DateTime month)
    {
        var startOfMonth = new DateTime(month.Year, month.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

        return [.. _database.Table<Tasks>().Where(task => task.ScheduledDate >= startOfMonth && task.ScheduledDate <= endOfMonth)];
    }

    // Fetch a specific pet by ID
    public Pet GetPetById(int id)
    {
        // Fetch the pet by ID
        return _database.Table<Pet>().FirstOrDefault(p => p.Id == id);
    }

    // Fetch medical records for a specific pet
    public List<MedicalRecord> GetMedicalRecordsForPet(int petId)
    {
        return [.. _database.Table<MedicalRecord>().Where(r => r.PetId == petId)];
    }

    // Fetch all medical records
    public List<MedicalRecord> GetAllMedicalRecords()
    {
        return [.. _database.Table<MedicalRecord>()];
    }

    // Fetch a specific medical record by ID
    public MedicalRecord GetMedicalRecordById(int id)
    {
        return _database.Table<MedicalRecord>().FirstOrDefault(r => r.Id == id);
    }

    // Delete a medical record by ID
    public void DeleteMedicalRecord(int id)
    {
        var record = GetMedicalRecordById(id);
        if (record != null)
        {
            _database.Delete(record);
        }
    }
}
