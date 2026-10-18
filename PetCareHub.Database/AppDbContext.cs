using Microsoft.EntityFrameworkCore;
using PetCareHub.Database.Entities;

namespace PetCareHub.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<HealthLog> HealthLogs => Set<HealthLog>();
    public DbSet<Reminder> Reminders => Set<Reminder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pet>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasQueryFilter(x => !x.IsDeleted);
            e.HasMany(x => x.HealthLogs).WithOne(x => x.Pet).HasForeignKey(x => x.PetId);
            e.HasMany(x => x.Reminders).WithOne(x => x.Pet).HasForeignKey(x => x.PetId);
        });

        modelBuilder.Entity<HealthLog>().HasKey(x => x.Id);
        modelBuilder.Entity<Reminder>().HasKey(x => x.Id);
        modelBuilder.Entity<User>().HasKey(x => x.Id);
    }
}
