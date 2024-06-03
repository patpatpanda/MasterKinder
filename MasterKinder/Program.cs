using Microsoft.Extensions.DependencyInjection;
using MasterKinder.Data;
using MasterKinder.Pages;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"),
    sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5, // Max antal �terf�rs�k
            maxRetryDelay: TimeSpan.FromSeconds(30), // Maximal f�rdr�jning mellan �terf�rs�k
            errorNumbersToAdd: null // Felnummer att inkludera i �terf�rs�k
        );
    }));

builder.Services.AddScoped<CsvService>();
builder.Services.AddSingleton<MasterKinder.Services.AuthService>();
builder.Services.AddSingleton<MasterKinder.Services.PowerBIService>();

var app = builder.Build();

// Load CSV Data

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// https://virki.se/
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
