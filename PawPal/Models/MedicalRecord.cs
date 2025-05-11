namespace PawPal.Models;

public class MedicalRecord
{
    public int Id { get; set; }
    public int PetId { get; set; } // Foreign key to Pet

    public DateTime RecordDate { get; set; }
    public string RecordType { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? Prescriptions { get; set; }
    public string? Doctor { get; set; }

    // ✅ Navigation Property (back to Pet)
    public Pet Pet { get; set; } = null!;
}
