using HtmlAgilityPack;
using KinderReader;
using MasterKinder.Data;
using MasterKinder.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

// Fetch and process PDF files
await FetchAndProcessPdfFilesAsync(app.Services);

app.Run();

async Task FetchAndProcessPdfFilesAsync(IServiceProvider services)
{
    var pdfUrls = await GetPdfUrlsFromPage("https://ssan.stockholm.se/anonym/webdokument/_layouts/15/start.aspx#/Delade%20dokument/Forms/AllItems.aspx?RootFolder=%2Fanonym%2Fwebdokument%2FDelade%20dokument%2FF%C3%B6rskolor%2F2024%2FS%C3%B6dermalm&FolderCTID=0x01200015B00A3B7947284E8A98F455403CF440&View=%7BCEB0BF65%2D2CB1%2D4A7B%2DA2B3%2DD82EE112AAA7%7D");

    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MrDb>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>(); // Use appropriate type if needed

    foreach (var pdfUrl in pdfUrls)
    {
        var forskolan = await FindForskolanByUrl(context, pdfUrl, logger);
        if (forskolan != null)
        {
            logger.LogInformation($"Processing PDF for Forskolan: {forskolan.Namn}");
            await PdfReader.ReadPdfFromUrl(pdfUrl, services, forskolan.Id);
        }
        else
        {
            logger.LogWarning($"No Forskolan found for PDF URL: {pdfUrl}");
        }
    }
}



async Task<List<string>> GetPdfUrlsFromPage(string pageUrl)
{
    var pdfUrls = new List<string>();
    var web = new HtmlWeb();
    var document = await web.LoadFromWebAsync(pageUrl);

    var pdfLinks = document.DocumentNode.SelectNodes("//a[contains(@href, '.pdf')]");

    if (pdfLinks != null)
    {
        foreach (var link in pdfLinks)
        {
            var pdfUrl = link.GetAttributeValue("href", string.Empty);
            if (!string.IsNullOrEmpty(pdfUrl))
            {
                pdfUrls.Add(pdfUrl);
            }
        }
    }

    return pdfUrls;
}

async Task<Forskolan> FindForskolanByUrl(MrDb context, string pdfUrl, ILogger logger)
{
    logger.LogInformation($"Attempting to find Forskolan for PDF URL: {pdfUrl}");

    var name = ExtractNameFromUrl(pdfUrl);
    logger.LogInformation($"Extracted name from URL: {name}");

    if (!string.IsNullOrEmpty(name))
    {
        var forskolan = await context.Forskolans
            .Where(f => f.Namn.Contains(name))
            .FirstOrDefaultAsync();

        if (forskolan != null)
        {
            logger.LogInformation($"Found Forskolan: {forskolan.Namn}");
            return forskolan;
        }
        else
        {
            logger.LogWarning($"No Forskolan found for name: {name}");
        }
    }
    else
    {
        logger.LogWarning($"Could not extract name from URL: {pdfUrl}");
    }

    return null;
}

string ExtractNameFromUrl(string pdfUrl)
{
    var parts = pdfUrl.Split(new[] { '/', '-' }, StringSplitOptions.RemoveEmptyEntries);
    var namePart = parts.LastOrDefault(part => part.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase));

    if (namePart != null)
    {
        var name = namePart.Replace(".pdf", "", StringComparison.OrdinalIgnoreCase)
                           .Replace("-", " ");

        return name;
    }

    return string.Empty;
}


