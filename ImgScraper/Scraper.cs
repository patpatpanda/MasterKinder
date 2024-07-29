using System.Net;
using HtmlAgilityPack;
using KinderReader.Models;
using MasterKinder.Data;
using MasterKinder.Models;
using Microsoft.EntityFrameworkCore;

public class Scraper
{
    private readonly MrDb _context;
    private readonly HttpClient _httpClient;

    public Scraper(MrDb context)
    {
        _context = context;
        _httpClient = new HttpClient();
    }

    public async Task<bool> ScrapeImageAndUpdate(int id)
    {
        string[] urls = {
        $"https://forskola.stockholm/hitta-forskola/forskola/{id}",
        $"https://forskola.stockholm/hitta-forskola/pedagogisk-omsorg/{id}"
    };

        foreach (var url in urls)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var document = new HtmlDocument();
                    document.LoadHtml(content);

                    // Kontrollera om sidan är tom genom att kontrollera ett specifikt element som borde finnas
                    if (string.IsNullOrWhiteSpace(GetInnerText(document, "//h1")))
                    {
                        Console.WriteLine($"No content found for ID: {id} from URL: {url}. Skipping.");
                        continue;
                    }

                    var adress = HtmlDecode(GetInnerText(document, "//p[@class='mb-small']"));
                    var bildUrl = GetAttribute(document, "//img[contains(@src, '/optimized/serviceunitspage/filer/hitta')]", "src");

                    if (!string.IsNullOrEmpty(adress) && !string.IsNullOrEmpty(bildUrl))
                    {
                        // Bygg den fullständiga URLen för bilden
                        var fullBildUrl = new Uri(new Uri(url), bildUrl).ToString();

                        var forskolan = await _context.Forskolans.FirstOrDefaultAsync(f => f.Adress == adress);
                        if (forskolan != null)
                        {
                            forskolan.BildUrl = fullBildUrl;
                            await _context.SaveChangesAsync();
                            Console.WriteLine($"Successfully updated image for Forskolan with address: {adress}");
                            return true; // Indikerar att skrapningen lyckades
                        }
                        else
                        {
                            Console.WriteLine($"No Forskolan found with address: {adress}");
                        }
                    }
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"ID: {id} not found (404) at URL: {url}. Skipping.");
                    continue;
                }
                else
                {
                    Console.WriteLine($"Failed to fetch data for ID: {id} from URL: {url} with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing ID: {id} from URL: {url}, Exception: {ex.Message}");
            }
        }

        return false; // Indikerar att skrapningen misslyckades eller sidan var tom
    }

    private string GetInnerText(HtmlDocument document, string xpath)
    {
        var node = document.DocumentNode.SelectSingleNode(xpath);
        if (node == null)
        {
            Console.WriteLine($"Node not found for XPath: {xpath}");
            return string.Empty;
        }
        return node.InnerText.Trim();
    }

    private string HtmlDecode(string text)
    {
        return WebUtility.HtmlDecode(text);
    }

    private int ParseInt(string text)
    {
        int.TryParse(text, out int result);
        return result;
    }

    private double ParseDouble(string text)
    {
        double.TryParse(text, out double result);
        return result;
    }

    private double ParsePercentage(string text)
    {
        text = text.TrimEnd('%');
        return ParseDouble(text);
    }

    private string GetAttribute(HtmlDocument document, string xpath, string attribute)
    {
        var node = document.DocumentNode.SelectSingleNode(xpath);
        if (node == null)
        {
            Console.WriteLine($"Node not found for XPath: {xpath}");
            return string.Empty;
        }
        return node.GetAttributeValue(attribute, string.Empty).Trim();
    }

}
