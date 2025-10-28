using dotnetfeatures.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components; // Added for RenderMode
using dotnetfeatures; // Added for CustomUserService

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<CustomUserService>();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
//.RegisterPersistentService<CustomUserService>(RenderMode.InteractiveServer); // Removed InteractiveWebAssemblyComponents (no WASM project present)
builder.Services.AddValidation();                                                                                 // .AddInteractiveWebAssemblyComponents(); // Removed to avoid404 _framework/dotnet.js
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode() // Removed .AddInteractiveWebAssemblyRenderMode()
    .AddInteractiveWebAssemblyRenderMode(); // Requires separate Blazor WebAssembly project providing boot resources

app.Run();
