using Microsoft.EntityFrameworkCore;
using PawPal.Models;

namespace PawPal.Data;

public class AppDataContext : DbContext
{
    public DbSet<Pet> Pets { get; set; }
    public DbSet<PetTask> PetTasks { get; set; }
    public DbSet<VetContact> VetContacts { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }

    private readonly string _dbPath;

    public AppDataContext(string dbPath)
    {
        _dbPath = dbPath;
        Database.EnsureCreated(); // Creates database if it doesn't exist
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Filename={_dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // **Relationships and Foreign Keys**
        modelBuilder.Entity<PetTask>()
            .HasOne(pt => pt.Pet)
            .WithMany(p => p.PetTasks)
            .HasForeignKey(pt => pt.PetId)
            .OnDelete(DeleteBehavior.Cascade); // Deleting a Pet deletes its Tasks

        modelBuilder.Entity<MedicalRecord>()
            .HasOne(mr => mr.Pet)
            .WithMany(p => p.MedicalRecords)
            .HasForeignKey(mr => mr.PetId)
            .OnDelete(DeleteBehavior.Cascade); // Deleting a Pet deletes its Medical Records

        modelBuilder.Entity<VetContact>()
            .HasKey(vc => vc.VetId); // Explicitly define VetId as PK if needed
    }
}
