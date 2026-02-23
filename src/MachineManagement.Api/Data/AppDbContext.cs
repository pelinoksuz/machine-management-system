using MachineManagement.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace MachineManagement.Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Machine> Machines => Set<Machine>();
    public DbSet<MachineComponent> Components => Set<MachineComponent>();
    public DbSet<MachineConfiguration> Configurations => Set<MachineConfiguration>();
    public DbSet<MaintenancePlan> MaintenancePlans => Set<MaintenancePlan>();
    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
    public DbSet<Incident> Incidents => Set<Incident>();
    public DbSet<TelemetryReading> TelemetryReadings => Set<TelemetryReading>();
    public DbSet<Alert> Alerts => Set<Alert>();
    public DbSet<FileDocument> Files => Set<FileDocument>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasMany(x => x.Roles).WithMany();

        modelBuilder.Entity<MachineComponent>()
            .HasOne(c => c.Machine)
            .WithMany(m => m.Components)
            .HasForeignKey(c => c.MachineId);

        modelBuilder.Entity<MachineConfiguration>()
            .HasOne(c => c.Machine)
            .WithOne(m => m.Configuration)
            .HasForeignKey<MachineConfiguration>(c => c.MachineId);

        modelBuilder.Entity<WorkOrder>()
            .HasOne(x => x.Machine)
            .WithMany()
            .HasForeignKey(x => x.MachineId);

        modelBuilder.Entity<Incident>()
            .HasOne(x => x.Machine)
            .WithMany()
            .HasForeignKey(x => x.MachineId);

        modelBuilder.Entity<TelemetryReading>()
            .HasOne(x => x.Machine)
            .WithMany()
            .HasForeignKey(x => x.MachineId);

        modelBuilder.Entity<Alert>()
            .HasOne(x => x.Machine)
            .WithMany()
            .HasForeignKey(x => x.MachineId);

        modelBuilder.Entity<FileDocument>()
            .HasOne(x => x.Machine)
            .WithMany()
            .HasForeignKey(x => x.MachineId);
    }
}
