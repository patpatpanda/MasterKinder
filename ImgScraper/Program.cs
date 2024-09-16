using KinderReader;
using MasterKinder.Data;
using MasterKinder.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext with SQL Server connection
builder.Services.AddDbContext<MrDb>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultSQLConnection")));

// Add the scraper service
builder.Services.AddScoped<Scraper>();

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

// Hämta start- och slut-ID från kommandoradsargument
var commandLineArgs = Environment.GetCommandLineArgs();
int.TryParse(commandLineArgs.ElementAtOrDefault(1), out int startId);
int.TryParse(commandLineArgs.ElementAtOrDefault(2), out int endId);

// Om argument inte tillhandahålls, sätt startId och endId
if (startId == 0) startId = 15000;
if (endId == 0) endId = 16000;

// Kör huvudskrapningsmetoden
await MainMethod(app.Services, startId, endId);

app.Run();

async Task MainMethod(IServiceProvider services, int startId, int endId)
{
    // Definiera batch-storlek
    const int batchSize = 5;

    // Räkna alla poster i Forskolans
    int totalForskolans = await CountAllForskolansAsync(services);
    Console.WriteLine($"Total number of Forskolans: {totalForskolans}");

    // Kör skrapning i batchar tills alla sidor har skrapats
    while (startId <= endId)
    {
        // Bestäm slutet för den aktuella batchen
        int currentEndId = Math.Min(startId + batchSize - 1, endId);

        // Kör skrapning för den aktuella batchen
        await RunScraperAsync(services, startId, currentEndId);

        // Uppdatera start-ID för nästa batch
        startId = currentEndId + 1;

        // Valfri fördröjning mellan batchar för att undvika överbelastning av servern
        await Task.Delay(1000); // 1 sekund fördröjning mellan varje batch
    }

    Console.WriteLine("Scraping process completed for all pages.");
}

async Task<int> CountAllForskolansAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MrDb>();
    return await context.Forskolans.CountAsync();
}

async Task RunScraperAsync(IServiceProvider services, int startId, int endId)
{
    using var scope = services.CreateScope();
    var scraper = scope.ServiceProvider.GetRequiredService<Scraper>();

    int totalEmptyPages = 0;
    int totalPages = endId - startId + 1;

    for (int id = startId; id <= endId; id++)
    {
        bool isSuccess = await scraper.ScrapeImageAndUpdate(id);

        if (!isSuccess)
        {
            totalEmptyPages++;
            Console.WriteLine($"Empty page count: {totalEmptyPages}");
        }

        await Task.Delay(100); // Fördröjning för att undvika överbelastning

        // Valfritt: Lägg till logik för att skriva ut framsteg
        if (id % 100 == 0)
        {
            Console.WriteLine($"Processed {id - startId + 1} of {totalPages} pages.");
        }
    }

    Console.WriteLine($"Scraping process completed for range {startId} to {endId}. Total empty pages: {totalEmptyPages}");
}
