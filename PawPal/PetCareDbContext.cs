using Microsoft.EntityFrameworkCore;

namespace PawPal;

public class PetCareDbContext(DbContextOptions<PetCareDbContext> options) : DbContext(options)
{
    public DbSet<Pet>? Pets { get; set; }
}