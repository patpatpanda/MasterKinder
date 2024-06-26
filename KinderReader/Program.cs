using KinderReader;
using MasterKinder.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext with SQL Server connection
builder.Services.AddDbContext<MrDb>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultSQLConnection")));

// Add the scraper service
builder.Services.AddScoped<Scraper>();

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

// Run the scraper with start and end IDs
await RunScraperAsync(app.Services, 10500, 10700);

// Remove duplicates
await RemoveDuplicatesAsync(app.Services);

app.Run();

async Task RunScraperAsync(IServiceProvider services, int startId, int endId)
{
    using var scope = services.CreateScope();
    var scraper = scope.ServiceProvider.GetRequiredService<Scraper>();
    await scraper.Scrape(startId, endId);
}

async Task RemoveDuplicatesAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MrDb>();

    var duplicates = await context.Forskolans
        .GroupBy(f => new { f.Namn, f.Adress })
        .Where(g => g.Count() > 1)
        .SelectMany(g => g.OrderBy(x => x.Id).Skip(1))
        .ToListAsync();

    context.Forskolans.RemoveRange(duplicates);
    await context.SaveChangesAsync();
}