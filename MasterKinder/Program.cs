using MasterKinder.Data;
using MasterKinder.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers(); // Enable API controllers

// Configure DbContexts
builder.Services.AddDbContext<MrDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection")));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection")));

builder.Services.AddMemoryCache(); // Lägg till denna rad för att registrera IMemoryCache
builder.Services.AddScoped<CsvService>();
builder.Services.AddSingleton<MasterKinder.Services.AuthService>();
builder.Services.AddSingleton<MasterKinder.Services.PowerBIService>();
builder.Services.AddScoped<ISchoolService, SchoolService>(); // Registrera SchoolService

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

app.UseHttpsRedirection();
app.UseStaticFiles(); // Serve static files from wwwroot
app.UseRouting();
app.UseCors("AllowAll"); // Make sure CORS is used
app.UseAuthorization();

app.MapControllers(); // Enable API routes

// Serve the React app's index.html as a fallback for all other routes
app.MapFallbackToFile("/index.html");

app.Run();
