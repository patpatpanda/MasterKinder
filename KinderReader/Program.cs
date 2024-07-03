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

// Run the main method
await MainMethod(app.Services);

app.Run();

async Task MainMethod(IServiceProvider services)
{
    // Räkna alla poster i Forskolans
    int totalForskolans = await CountAllForskolansAsync(services);
    Console.WriteLine($"Total number of Forskolans: {totalForskolans}");

    // Exempel på att köra skrapning och andra operationer
    await RunScraperAsync(services, 10987, 10988);
   
   
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

    int batchSize = 5; // Antal sidor per omgång
    int maxEmptyPages = 50; // Max antal tomma sidor innan avbrott
    int emptyPageCount = 0;
    int totalEmptyPages = 0; // Totalt antal tomma sidor

    for (int batchStart = startId; batchStart <= endId; batchStart += batchSize)
    {
        int batchEnd = Math.Min(batchStart + batchSize - 1, endId);

        for (int id = batchStart; id <= batchEnd; id++)
        {
            bool isSuccess = await scraper.Scrape(id);

            if (!isSuccess)
            {
                emptyPageCount++;
                totalEmptyPages++;
                Console.WriteLine($"Empty page count: {emptyPageCount} (Total empty pages: {totalEmptyPages})");
            }
            else
            {
                emptyPageCount = 0; // Återställ räknaren om en sida har innehåll
            }

            // Om vi har nått max antal tomma sidor, sluta skrapa
            if (totalEmptyPages >= maxEmptyPages)
            {
                Console.WriteLine($"Max empty pages reached. Stopping scraper at ID: {id}.");
                return;
            }

            // Valfri fördröjning för att minska belastningen på servern
            await Task.Delay(100); // 100 ms fördröjning
        }

        Console.WriteLine($"Completed batch from ID: {batchStart} to ID: {batchEnd}. Restarting process.");
    }

    Console.WriteLine("Scraping process completed.");
}
