using SQLite;

namespace PawPal.Models;

//TODO: photo
public class Pet
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Species { get; set; } = string.Empty; 

    public string? Breed { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string? MedicalRecords { get; set; }

    public int Age { get; set; }

    public string? NextTask { get; set; } // Nullable to handle pets without tasks
    
    public override string ToString()
    {
        return $"{Name} ({Species}, {Age}, {Breed ?? "Unknown Breed"})";
    }
}
