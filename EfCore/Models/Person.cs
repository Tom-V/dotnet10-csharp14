using System.ComponentModel.DataAnnotations;

namespace EfCore.Models;

// Example entity with complex types
public class Person
{
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    // Complex type - Address (table splitting)
    public Address? Address { get; set; }

    // Complex type stored as JSON
    public ContactInfo? ContactInfo { get; set; }

    // Struct complex type
    public PersonStats Stats { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation property
    public List<Order> Orders { get; set; } = new();
    public bool IsDeleted { get; internal set; }
    public string TenantId { get; internal set; }
}

// Complex type for table splitting
public class Address
{
    [Required]
    public string Street { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = string.Empty;

    [Required]
    public string PostalCode { get; set; } = string.Empty;

    public string? Country { get; set; }
}

// Complex type for JSON storage
public class ContactInfo
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Website { get; set; }
    public List<string> SocialMediaHandles { get; set; } = new();
}

// Struct complex type (EF Core 10 improvement)
public readonly struct PersonStats
{
    public PersonStats(int totalOrders, decimal totalSpent, DateTime lastOrderDate)
    {
        TotalOrders = totalOrders;
        TotalSpent = totalSpent;
        LastOrderDate = lastOrderDate;
    }

    public int TotalOrders { get; }
    public decimal TotalSpent { get; }
    public DateTime LastOrderDate { get; }
}