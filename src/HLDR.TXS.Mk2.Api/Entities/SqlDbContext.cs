
using Microsoft.EntityFrameworkCore;

namespace AccessControlSystem.Api.Entities;

public class SqlDbContext : DbContext
{
    public SqlDbContext(DbContextOptions<SqlDbContext> options)
        : base(options)
    {
    }

    public DbSet<AccessControlDevice> Devices { get; set; }

    public DbSet<Slot> Slots { get; set; }

    public DbSet<AccessCard> AccessCards { get; set; }

    public DbSet<AccessMapping> Mappings { get; set; }

    public string SchemaName => "accessControl";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);
        modelBuilder.Entity<AccessControlDevice>()
            .HasMany(d => d.Slots)
            .WithOne(s => s.Device)
            .IsRequired();

        modelBuilder.Entity<AccessControlDevice>()
            .ToTable("devices");

        modelBuilder.Entity<Slot>()
            .HasOne(s => s.Device)
            .WithMany(d => d.Slots)
            .HasForeignKey(s => s.DeviceId)
            .IsRequired();

        modelBuilder.Entity<Slot>()
            .HasMany(s => s.AccessCards)
            .WithMany(t => t.Slots)
            .UsingEntity<AccessMapping>();

        modelBuilder.Entity<Slot>()
            .ToTable("slots");

        modelBuilder.Entity<AccessCard>()
            .ToTable("accessCards");

        modelBuilder.Entity<AccessMapping>()
            .ToTable("accessMapping");
    }
}