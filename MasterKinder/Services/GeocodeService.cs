using System.Net.Http;
using System.Threading.Tasks;
using MasterKinder.Models;
using Newtonsoft.Json.Linq;
namespace MasterKinder.Services;
public class GeocodeService : IGeocodeService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "fe1de6b82f1d42e9a8bbdce411d045ca"; // Byt ut med din API-nyckel

    public GeocodeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GeocodeResult> GeocodeAddress(string address)
    {
        var url = $"https://api.opencagedata.com/geocode/v1/json?q={Uri.EscapeDataString(address)}&key={_apiKey}";
        Console.WriteLine($"Generated URL: {url}");  // Log the generated URL

        var response = await _httpClient.GetStringAsync(url);
        var json = JObject.Parse(response);

        var results = json["results"];
        if (results == null || !results.HasValues)
        {
            return null;
        }

        var coordinates = results[0]?["geometry"];
        if (coordinates == null)
        {
            return null;
        }

        return new GeocodeResult
        {
            Latitude = (double)coordinates["lat"],
            Longitude = (double)coordinates["lng"]
        };
    }
}

    public class GeocodeResult
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
