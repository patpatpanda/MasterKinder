using MasterKinder.Data;
using MasterKinder.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    }); // Enable API controllers with JSON options

// Configure DbContexts
builder.Services.AddDbContext<MrDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter(); // L�gg till denna rad

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<MrDb>();

builder.Services.AddMemoryCache(); // L�gg till denna rad f�r att registrera IMemoryCache

builder.Services.AddHttpClient<GeocodeService>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage(); // L�gg till denna rad f�r att visa detaljerade fel i utvecklingsl�ge
}
app.Use(async (context, next) =>
{
    Console.WriteLine($"Handling request: {context.Request.Path}");
    await next.Invoke();
    Console.WriteLine($"Finished handling request.");
});

app.UseHttpsRedirection();
app.UseStaticFiles(); // Serve static files from wwwroot
app.UseRouting();
app.UseCors("AllowAll"); // Make sure CORS is used
app.UseAuthentication(); // L�gg till detta f�r att aktivera autentisering
app.UseAuthorization();

app.MapControllers(); // Enable API routes

// Map Razor Pages
app.MapRazorPages();

// Serve the React app's index.html as a fallback for all other routes


app.Run();