using EfCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace EfCore.Data;

public class AppDbContext : DbContext
{
    public DbSet<Person> People { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Use SQL Server with in-memory database for demonstration
        optionsBuilder.UseSqlServer("Server=.;Database=EfCore10Demo;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;", options => options.UseParameterizedCollectionMode(ParameterTranslationMode.Constant));
        // Enable sensitive data logging for development
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
    .HasQueryFilter("SoftDeletionFilter", b => !b.IsDeleted)
    .HasQueryFilter("TenantFilter", b => b.TenantId == "5646546465");
        // Configure Person entity
        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(p => p.Id);

            // Table splitting - Address complex type shares the same table
            entity.ComplexProperty(p => p.Address, addressBuilder =>
            {
                // Map to the same table (People) with column prefixes
                addressBuilder.Property(a => a.Street).HasColumnName("Address_Street");
                addressBuilder.Property(a => a.City).HasColumnName("Address_City");
                addressBuilder.Property(a => a.State).HasColumnName("Address_State");
                addressBuilder.Property(a => a.PostalCode).HasColumnName("Address_PostalCode");
                addressBuilder.Property(a => a.Country).HasColumnName("Address_Country");
            });

            // JSON storage - ContactInfo complex type stored as JSON
            entity.ComplexProperty(p => p.ContactInfo, contactBuilder =>
            {
                contactBuilder.ToJson("ContactInfoJson");
            });

            // Struct complex type - PersonStats
            entity.ComplexProperty(p => p.Stats, statsBuilder =>
            {
                statsBuilder.Property(s => s.TotalOrders).HasColumnName("Stats_TotalOrders");
                statsBuilder.Property(s => s.TotalSpent).HasColumnName("Stats_TotalSpent").HasPrecision(18, 2);
                statsBuilder.Property(s => s.LastOrderDate).HasColumnName("Stats_LastOrderDate");
            });

            // Configure relationships
            entity.HasMany(p => p.Orders)
                  .WithOne(o => o.Person)
                  .HasForeignKey(o => o.PersonId);
        });

        // Configure Order entity
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity.Property(o => o.TotalAmount)
                  .HasPrecision(18, 2);

            // JSON storage - ShippingInfo complex type stored as JSON
            entity.ComplexProperty(o => o.ShippingInfo, shippingBuilder =>
            {
                shippingBuilder.ToJson("ShippingInfoJson");
            });

            // Configure relationships
            entity.HasMany(o => o.OrderItems)
                  .WithOne(oi => oi.Order)
                  .HasForeignKey(oi => oi.OrderId);
        });

        // Configure OrderItem entity
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(oi => oi.Id);

            entity.Property(oi => oi.UnitPrice)
                  .HasPrecision(18, 2);
        });

        base.OnModelCreating(modelBuilder);
    }
}