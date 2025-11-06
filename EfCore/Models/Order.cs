using System.ComponentModel.DataAnnotations;

namespace EfCore.Models;

public class Order
{
    public int Id { get; set; }
    
    [Required]
    public string OrderNumber { get; set; } = string.Empty;
    
    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;
    
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    
    [Range(0, double.MaxValue)]
    public decimal TotalAmount { get; set; }
    
    public OrderStatus Status { get; set; }
    
    // Complex type for shipping information (JSON)
    public ShippingInfo? ShippingInfo { get; set; }
    
    public List<OrderItem> OrderItems { get; set; } = new();
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    
    [Required]
    public string ProductName { get; set; } = string.Empty;
    
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

// Complex type for JSON storage
public class ShippingInfo
{
    public string? TrackingNumber { get; set; }
    public string? Carrier { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
    public Address? ShippingAddress { get; set; }
}

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}