using SQLite;

namespace PawPal.Models;

public class VetContact
{
    [PrimaryKey, AutoIncrement]
    public int VetId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Notes { get; set; }
    public bool IsEmergency { get; set; }
}
