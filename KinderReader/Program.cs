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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext with SQL Server connection
builder.Services.AddDbContext<MrDb>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultSQLConnection")));

// Add HttpClient for making API requests
builder.Services.AddHttpClient();

var app = builder.Build();

// Update geocode data
await UpdateGeocodeDataAsync(app.Services);

app.Run();

async Task UpdateGeocodeDataAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MrDb>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var httpClient = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();

    var forskolor = await context.Forskolans.ToListAsync();

    foreach (var forskola in forskolor)
    {
        if (forskola.Latitude == 0 && forskola.Longitude == 0)
        {
            if (string.IsNullOrWhiteSpace(forskola.Adress))
            {
                Console.WriteLine($"Skipping empty address for Forskolan ID: {forskola.Id}");
                continue;
            }

            var coordinates = await GeocodeAddressAsync(forskola.Adress, configuration, httpClient);
            if (coordinates != null)
            {
                forskola.Latitude = coordinates.Latitude;
                forskola.Longitude = coordinates.Longitude;
                context.Entry(forskola).State = EntityState.Modified;
            }
        }
    }

    await context.SaveChangesAsync();
}

async Task<GeocodeResult> GeocodeAddressAsync(string address, IConfiguration configuration, HttpClient httpClient)
{
    if (string.IsNullOrWhiteSpace(address))
    {
        Console.WriteLine("Address is null or empty.");
        return null;
    }

    // Förbättra adressen genom att lägga till stad och land (om det behövs)
    string improvedAddress = ImproveAddress(address);

    var apiKey = configuration["GoogleMapsApiKey"];
    var encodedAddress = Uri.EscapeDataString(improvedAddress);
    var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={encodedAddress}&key={apiKey}";

    Console.WriteLine($"Geocoding URL: {url}");

    try
    {
        var response = await httpClient.GetStringAsync(url);
        var json = JObject.Parse(response);

        if (json["results"] == null || !json["results"].Any())
        {
            Console.WriteLine($"No geocoding results for address: {improvedAddress}");
            return null;
        }

        var location = json["results"]?[0]?["geometry"]?["location"];
        if (location == null)
        {
            Console.WriteLine($"No location data in geocoding results for address: {improvedAddress}");
            return null;
        }

        return new GeocodeResult
        {
            Latitude = (double)location["lat"],
            Longitude = (double)location["lng"]
        };
    }
    catch (HttpRequestException e)
    {
        Console.WriteLine($"Request error: {e.Message}");
        return null;
    }
}

string ImproveAddress(string address)
{
    // Ta bort onödiga tecken och lägg till stad och land
    string improvedAddress = address;

    // Lägg till stad och land om de saknas
    if (!address.Contains("Stockholm"))
    {
        improvedAddress += ", Stockholm";
    }
    if (!address.Contains("Sweden"))
    {
        improvedAddress += ", Sweden";
    }

    return improvedAddress.Trim();
}

public class GeocodeResult
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
