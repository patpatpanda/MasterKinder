using MasterKinder.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using KinderReader;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext with SQL Server connection
builder.Services.AddDbContext<MrDb>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultSQLConnection")));

// Add the scraper services
builder.Services.AddScoped<GoScraper>();
builder.Services.AddScoped<PdfScrapingService>();

// Add HttpClient for making API requests
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Run the main method
await MainMethod(app.Services);

app.Run();

async Task MainMethod(IServiceProvider services)
{
    // Räkna alla poster i Forskolans
    int totalForskolans = await CountAllForskolansAsync(services);
    Console.WriteLine($"Total number of Forskolans: {totalForskolans}");

    // Exempel på att köra skrapning och andra operationer
    await RunScraperAsync(services);
}

async Task<int> CountAllForskolansAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MrDb>();
    return await context.Forskolans.CountAsync();
}

async Task RunScraperAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var scraper = scope.ServiceProvider.GetRequiredService<GoScraper>();

    // Skrapa PDF-filer från den angivna URLen
    bool isSuccess = await scraper.Scrape(0);

    if (isSuccess)
    {
        Console.WriteLine("PDF scraping and saving completed successfully.");
    }
    else
    {
        Console.WriteLine("PDF scraping failed.");
    }
}
