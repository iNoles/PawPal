using SQLite;

namespace PawPal;

public class Pet
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Species { get; set; } = string.Empty; 

    public string? Breed { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string? MedicalRecords { get; set; }
    public override string ToString()
    {
        return $"{Name} ({Species}, {Breed ?? "Unknown Breed"})";
    }
}
