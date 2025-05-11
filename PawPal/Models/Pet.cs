namespace PawPal.Models;

//TODO: Add photo
public class Pet
{
    public int Id { get; set; } // Primary key (auto-increment by default in EF Core)

    public string Name { get; set; } = string.Empty;

    public string Species { get; set; } = string.Empty;

    public string? Breed { get; set; }

    public DateTime DateOfBirth { get; set; }

    public int Age { get; set; }

    public string? NextTask { get; set; }

    // Optional notes about medical history (if needed, distinct from actual records)
    public string? MedicalNotes { get; set; }

    public List<MedicalRecord> MedicalRecords { get; set; } = [];

    public List<PetTask> PetTasks { get; set; } = [];

    public override string ToString()
    {
        return $"{Name} ({Species}, {Age}, {Breed ?? "Unknown Breed"})";
    }
}
