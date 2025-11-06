using Microsoft.EntityFrameworkCore;
using EfCore.Data;
using EfCore.Models;

Console.WriteLine("=== EF Core 10 Features Demonstration ===\n");

using var context = new AppDbContext();

// Ensure database is created
await context.Database.EnsureDeletedAsync();
await context.Database.EnsureCreatedAsync();

await DemonstrateComplexTypesWithTableSplitting();
await DemonstrateComplexTypesWithJson();
await DemonstrateStructComplexTypes();
await DemonstrateParameterizedCollectionTranslation();

Console.WriteLine("\n=== Demonstration Complete ===");

async Task DemonstrateComplexTypesWithTableSplitting()
{
    Console.WriteLine("1. Complex Types with Table Splitting");
    Console.WriteLine("=====================================");

    // Create a person with an address (table splitting)
    var person = new Person
    {
        FirstName = "John",
        LastName = "Doe",
        TenantId = "5646546465", // Fixed: Match the tenant filter in AppDbContext
        Address = new Address
        {
            Street = "123 Main St",
            City = "Seattle",
            State = "WA",
            PostalCode = "98101",
            Country = "USA"
        },
        ContactInfo = new ContactInfo
        {
            Email = "john.doe@example.com",
            PhoneNumber = "+1-555-0123",
            Website = "https://johndoe.com",
            SocialMediaHandles = ["@johndoe", "@john_doe_official"]
        },
        Stats = new PersonStats(0, 0m, DateTime.UtcNow)
    };

    context.People.Add(person);
    await context.SaveChangesAsync();

    // Query person with address (same table)
    var retrievedPerson = await context.People
        .Where(p => p.FirstName == "John")
        .FirstOrDefaultAsync();

    if (retrievedPerson?.Address != null)
    {
        Console.WriteLine($"Person: {retrievedPerson.FirstName} {retrievedPerson.LastName}");
        Console.WriteLine($"Address: {retrievedPerson.Address.Street}, {retrievedPerson.Address.City}, {retrievedPerson.Address.State}");
        Console.WriteLine($"Postal Code: {retrievedPerson.Address.PostalCode}");
    }

    Console.WriteLine();
}

async Task DemonstrateComplexTypesWithJson()
{
    Console.WriteLine("2. Complex Types stored as JSON");
    Console.WriteLine("===============================");

    // Query contact info stored as JSON
    var peopleWithGmail = await context.People
        .Where(p => p.ContactInfo!.Email!.Contains("example.com"))
        .ToListAsync();

    foreach (var person in peopleWithGmail)
    {
        if (person.ContactInfo != null)
        {
            Console.WriteLine($"Person: {person.FirstName} {person.LastName}");
            Console.WriteLine($"Email: {person.ContactInfo.Email}");
            Console.WriteLine($"Phone: {person.ContactInfo.PhoneNumber}");
            Console.WriteLine($"Social Media: [{string.Join(", ", person.ContactInfo.SocialMediaHandles)}]");
        }
    }

    // Create an order with shipping info (JSON) - only if people were found
    var firstPerson = peopleWithGmail.FirstOrDefault();
    if (firstPerson != null)
    {
        var order = new Order
        {
            OrderNumber = "ORD-2024-001",
            PersonId = firstPerson.Id,
            TotalAmount = 299.99m,
            Status = OrderStatus.Shipped,
            ShippingInfo = new ShippingInfo
            {
                TrackingNumber = "1Z999AA1234567890",
                Carrier = "UPS",
                ShippedDate = DateTime.UtcNow.AddDays(-2),
                EstimatedDelivery = DateTime.UtcNow.AddDays(1),
                ShippingAddress = new Address
                {
                    Street = "456 Oak Avenue",
                    City = "Portland",
                    State = "OR",
                    PostalCode = "97201",
                    Country = "USA"
                }
            }
        };

        order.OrderItems.Add(new OrderItem
        {
            ProductName = "Wireless Headphones",
            Quantity = 1,
            UnitPrice = 299.99m
        });

        context.Orders.Add(order);
        await context.SaveChangesAsync();

        // Query orders with JSON properties
        var shippedOrders = await context.Orders
            .Where(o => o.ShippingInfo!.Carrier == "UPS")
            .Include(o => o.OrderItems)
            .ToListAsync();

        foreach (var shippedOrder in shippedOrders)
        {
            Console.WriteLine($"Order: {shippedOrder.OrderNumber}");
            Console.WriteLine($"Carrier: {shippedOrder.ShippingInfo?.Carrier}");
            Console.WriteLine($"Tracking: {shippedOrder.ShippingInfo?.TrackingNumber}");
            Console.WriteLine($"Items: {string.Join(", ", shippedOrder.OrderItems.Select(oi => $"{oi.ProductName} (${oi.UnitPrice})"))}");
        }
    }
    else
    {
        Console.WriteLine("No people found with example.com email addresses.");
    }

    Console.WriteLine();
}

async Task DemonstrateStructComplexTypes()
{
    Console.WriteLine("3. Struct Complex Types (EF Core 10 improvement)");
    Console.WriteLine("================================================");

    // Update person stats (struct complex type)
    var person = await context.People.FirstAsync();
    var orderCount = await context.Orders.CountAsync(o => o.PersonId == person.Id);
    var totalSpent = await context.Orders
        .Where(o => o.PersonId == person.Id)
        .SumAsync(o => o.TotalAmount);
    var lastOrderDate = await context.Orders
        .Where(o => o.PersonId == person.Id)
        .MaxAsync(o => o.OrderDate);

    // Update with new struct value
    person.Stats = new PersonStats(orderCount, totalSpent, lastOrderDate);
    await context.SaveChangesAsync();

    // Query using struct properties
    var highValueCustomers = await context.People
        .Where(p => p.Stats.TotalSpent > 200m)
        .ToListAsync();

    foreach (var customer in highValueCustomers)
    {
        Console.WriteLine($"Customer: {customer.FirstName} {customer.LastName}");
        Console.WriteLine($"Total Orders: {customer.Stats.TotalOrders}");
        Console.WriteLine($"Total Spent: ${customer.Stats.TotalSpent:F2}");
        Console.WriteLine($"Last Order: {customer.Stats.LastOrderDate:yyyy-MM-dd}");
    }

    Console.WriteLine();
}

async Task DemonstrateParameterizedCollectionTranslation()
{
    Console.WriteLine("4. Improved Parameterized Collection Translation");
    Console.WriteLine("===============================================");

    // Add more test data
    var additionalPeople = new[]
    {
        new Person
        {
            FirstName = "Jane",
            LastName = "Smith",
            Address = new Address { Street = "789 Pine St", City = "San Francisco", State = "CA", PostalCode = "94102", Country = "USA" },
            ContactInfo = new ContactInfo { Email = "jane.smith@example.com" },
            Stats = new PersonStats(0, 0m, DateTime.UtcNow),
            TenantId = "5646546465",

        },
        new Person
        {
            FirstName = "Bob",
            LastName = "Johnson",
            Address = new Address { Street = "321 Elm St", City = "Austin", State = "TX", PostalCode = "73301", Country = "USA" },
            ContactInfo = new ContactInfo { Email = "bob.johnson@example.com" },
            Stats = new PersonStats(0, 0m, DateTime.UtcNow),
            TenantId = "5646546465",
        },
        new Person
        {
            FirstName = "Alice",
            LastName = "Williams",
            Address = new Address { Street = "654 Maple Ave", City = "Denver", State = "CO", PostalCode = "80201", Country = "USA" },
            ContactInfo = new ContactInfo { Email = "alice.williams@example.com" },
            Stats = new PersonStats(0, 0m, DateTime.UtcNow),
            TenantId = "5646546465",
        }
    };

    context.People.AddRange(additionalPeople);
    await context.SaveChangesAsync();

    // Get all person IDs
    var allPersonIds = await context.People.Select(p => p.Id).ToListAsync();

    // EF Core 10: Improved translation for parameterized collections
    // This demonstrates the Contains() improvement with collections
    var targetIds = allPersonIds.Take(3).ToList();

    Console.WriteLine($"Searching for people with IDs: [{string.Join(", ", targetIds)}]");

    var peopleInCollection = await context.People
        .Where(p => targetIds.Contains(p.Id))
        .Select(p => new { p.Id, p.FirstName, p.LastName, p.Address!.City })
        .ToListAsync();

    Console.WriteLine("Results from parameterized collection query:");
    foreach (var person in peopleInCollection)
    {
        Console.WriteLine($"  ID: {person.Id}, Name: {person.FirstName} {person.LastName}, City: {person.City}");
    }

    // Another example with string collection
    var targetStates = new List<string> { "WA", "CA", "TX" };

    Console.WriteLine($"\nSearching for people in states: [{string.Join(", ", targetStates)}]");

    var peopleInStates = await context.People
        .Where(p => targetStates.Contains(p.Address!.State))
        .Select(p => new { p.FirstName, p.LastName, p.Address!.State, p.Address.City })
        .ToListAsync();

    Console.WriteLine("Results from state collection query:");
    foreach (var person in peopleInStates)
    {
        Console.WriteLine($"  {person.FirstName} {person.LastName} in {person.City}, {person.State}");
    }

    // Complex example with order statuses
    var activeStatuses = new List<OrderStatus> { OrderStatus.Processing, OrderStatus.Shipped };

    var activeOrders = await context.Orders
        .Where(o => activeStatuses.Contains(o.Status))
        .Include(o => o.Person)
        .Select(o => new { o.OrderNumber, o.Status, CustomerName = $"{o.Person.FirstName} {o.Person.LastName}" })
        .ToListAsync();

    Console.WriteLine($"\nActive orders (Processing or Shipped):");
    foreach (var order in activeOrders)
    {
        Console.WriteLine($"  {order.OrderNumber} - {order.Status} - Customer: {order.CustomerName}");
    }

    Console.WriteLine();
}

//ignore query filters in ef core
var allBlogs = await context.People.IgnoreQueryFilters(["SoftDeletionFilter"]).ToListAsync();