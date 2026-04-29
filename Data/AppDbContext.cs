using Microsoft.EntityFrameworkCore;
using temperature_system.Models;

namespace temperature_system.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TemperatureReading> TemperatureReadings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TemperatureReading>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.DeviceId)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.TemperatureCelsius)
                  .IsRequired();

            entity.Property(e => e.Humidity)
                  .IsRequired(false);

            entity.Property(e => e.Timestamp)
                  .IsRequired();

            entity.Property(e => e.ReceivedAt)
                  .IsRequired();

            entity.HasIndex(e => new { e.DeviceId, e.Timestamp });
        });
    }
}