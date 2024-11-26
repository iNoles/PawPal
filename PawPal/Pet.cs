using System.ComponentModel.DataAnnotations;

namespace PawPal;

public class Pet
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)] // Optional: Sets a max length for the Name field in the database
    public string Name { get; set; } = string.Empty; // Non-nullable

    [Required]
    [MaxLength(30)] // Optional: Sets a max length for the Species field
    public string Species { get; set; } = string.Empty; // Required and non-nullable

    public string? Breed { get; set; } // Optional

    [Required]
    public DateTime DateOfBirth { get; set; } // Required and non-nullable

    public string? MedicalRecords { get; set; } // Optional

     // Navigation property to related tasks
    public List<Tasks> Tasks { get; set; } = []; // Each pet can have multiple tasks
}
