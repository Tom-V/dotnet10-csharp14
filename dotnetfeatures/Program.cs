using dotnetfeatures; // Added for CustomUserService
using dotnetfeatures.Components;
using dotnetfeatures.Services;
using Microsoft.AspNetCore.Components; // Added for RenderMode
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<CustomUserService>();

// Register custom problem details service for validation error customization
//builder.Services.AddSingleton<IProblemDetailsService, CustomProblemDetailsService>();
builder.Services.AddProblemDetails();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
//.RegisterPersistentService<CustomUserService>(RenderMode.InteractiveServer); // Removed InteractiveWebAssemblyComponents (no WASM project present)
builder.Services.AddValidation();         // .AddInteractiveWebAssemblyComponents(); // Removed to avoid404 _framework/dotnet.js
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//empty string to null example
app.MapPost("/todo", ([FromForm] Todo todo) => TypedResults.Ok(todo));

//validation example
app.MapPost("/products", (Product product) =>
{
    // Endpoint logic here
    return TypedResults.Ok(product);
});

// Complex validation example with multiple fields
app.MapPost("/users", (UserRegistration user) =>
{
    return TypedResults.Ok(new { message = "User created successfully", user });
});

app.MapGet("/json-item", (CancellationToken cancellationToken) =>
{
    async IAsyncEnumerable<HeartRateRecord> GetHeartRate(
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var heartRate = Random.Shared.Next(60, 100);
            yield return HeartRateRecord.Create(heartRate);
            await Task.Delay(2000, cancellationToken);
        }
    }

    return TypedResults.ServerSentEvents(GetHeartRate(cancellationToken),
                                                  eventType: "heartRate");
});


app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode() // Removed .AddInteractiveWebAssemblyRenderMode()
    .AddInteractiveWebAssemblyRenderMode(); // Requires separate Blazor WebAssembly project providing boot resources

app.Run();
public class Todo
{
    public int Id { get; set; }
    public DateOnly? DueDate { get; set; } // Empty strings map to `null`
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}
public record Product(
    [Required] string Name,
    [Range(1, 1000)] int Quantity);

// Complex validation model to demonstrate custom error responses
public record UserRegistration(
    [Required]
    [StringLength(50, MinimumLength = 2)]
    string Name,
    
    [Required]
    [EmailAddress]
    string Email,
    
    [Required]
  [Range(18, 120)]
    int Age,
    
    [Required]
    [StringLength(20, MinimumLength = 6)]
    string Username);