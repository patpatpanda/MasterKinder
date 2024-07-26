using KinderReader;
using MasterKinder.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext with SQL Server connection
builder.Services.AddDbContext<MrDb>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultSQLConnection")));

// Add the scraper service
builder.Services.AddScoped<Scraper>();

// Add the PDF scraping service
builder.Services.AddHttpClient();
builder.Services.AddTransient<PdfScrapingService>();

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

// Run the PDF scraping service
await RunPdfScraperAsync(app.Services);

app.Run();

async Task RunPdfScraperAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var pdfScrapingService = scope.ServiceProvider.GetRequiredService<PdfScrapingService>();
    try
    {
        // Pass your actual URL here
        await pdfScrapingService.ScrapeAndSavePdfData("https://ssan.stockholm.se/anonym/webdokument/Delade%20dokument/Forms/AllItems.aspx?RootFolder=%2Fanonym%2Fwebdokument%2FDelade%20dokument%2FF%C3%B6rskolor%2F2024%2FS%C3%B6dermalm&FolderCTID=0x01200015B00A3B7947284E8A98F455403CF440&View=%7BCEB0BF65%2D2CB1%2D4A7B%2DA2B3%2DD82EE112AAA7%7D\r\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while scraping PDFs: {ex.Message}");
    }
}