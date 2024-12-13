using SQLite;

namespace PawPal.Models;

public class MedicalRecord
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; } // Primary key, never null

    [Indexed]
    public int PetId { get; set; } // Foreign key, should not be null

    public DateTime RecordDate { get; set; } // Non-nullable, as a record must have a date

    public string RecordType { get; set; } = string.Empty; // Non-nullable, default to an empty string

    public string? Notes { get; set; } // Nullable, as notes might be optional

    public string? Prescriptions { get; set; } // Nullable, as not all records may have prescriptions

    public string? Doctor { get; set; } // Nullable, as the doctor field might not always be applicable
}
