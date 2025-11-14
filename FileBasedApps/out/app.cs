
using Humanizer;

var dotNet10Released = DateTimeOffset.Parse("2024-11-12");
var since = DateTimeOffset.Now - dotNet10Released;

Console.WriteLine($"It has been {since.Humanize()} since .NET 10 was released.");
Console.WriteLine("Hello, World!");
Console.WriteLine($"Current date: {DateTime.Now:yyyy-MM-dd}");
Console.WriteLine($"Running on .NET {Environment.Version}");
