using System.Linq;
using SQLite;

namespace PawPal;

public class DatabaseService
{
    private SQLiteConnection _database;

    public DatabaseService()
    {
        // Get the database file path
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "appdata.db");

        // Create the SQLite connection
        _database = new SQLiteConnection(databasePath);

        // Enable foreign key constraints
        _database.Execute("PRAGMA foreign_keys = ON;");

        // Create multiple tables in a single call
        _database.CreateTables<Pet, Tasks>();
    }

    public void InsertPet(Pet pet)
    {
        _database.Insert(pet);
    }

    public void InsertTasks(Tasks tasks)
    {
        _database.Insert(tasks);
    }

    public List<Pet> GetPets()
    {
        return [.. _database.Table<Pet>()];
    }

    public List<Tasks> GetTasksForPet(int petId)
    {
        return [.. _database.Table<Tasks>().Where(t => t.PetId == petId)];
    }
}
