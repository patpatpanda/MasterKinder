using System.Net;
using HtmlAgilityPack;
using KinderReader.Models;
using MasterKinder.Data;
using MasterKinder.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Scraper
{
    private readonly MrDb _context;
    private readonly HttpClient _httpClient;

    public Scraper(MrDb context)
    {
        _context = context;
        _httpClient = new HttpClient();
    }

    public async Task Scrape(int startId, int endId)
    {
        for (int id = startId; id <= endId; id++)
        {
            // Try both URL formats
            var urls = new[]
            {
                $"https://forskola.stockholm/hitta-forskola/forskola/{id}",
                $"https://forskola.stockholm/hitta-forskola/pedagogisk-omsorg/{id}"
            };

            bool success = false;
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

                        var forskolan = new Forskolan
                        {
                            Namn = HtmlDecode(GetInnerText(document, "//h1")),
                            Adress = HtmlDecode(GetInnerText(document, "//p[@class='mb-small']")),
                            Beskrivning = HtmlDecode(GetInnerText(document, "//p[@class='preamble']")),
                            TypAvService = HtmlDecode(GetInnerText(document, "//dt[contains(text(), 'Typ av service')]/following-sibling::dd")),
                            VerksamI = HtmlDecode(GetInnerText(document, "//dt[contains(text(), 'Verksam i')]/following-sibling::dd")),
                            Organisationsform = HtmlDecode(GetInnerText(document, "//dt[contains(text(), 'Organisationsform')]/following-sibling::dd")),
                            AntalBarn = ParseInt(HtmlDecode(GetInnerText(document, "//dt[contains(text(), 'Antal barn')]/following-sibling::dd"))),
                            AntalBarnPerArsarbetare = ParseDouble(HtmlDecode(GetInnerText(document, "//dt[contains(text(), 'Antal barn per årsarbetare')]/following-sibling::dd"))),
                            AndelLegitimeradeForskollarare = ParsePercentage(HtmlDecode(GetInnerText(document, "//dt[contains(text(), 'Andel legitimerade förskollärare')]/following-sibling::dd"))),
                            Webbplats = HtmlDecode(GetInnerText(document, "//dt[contains(text(), 'Webbplats')]/following-sibling::dd/a")),
                            InriktningOchProfil = HtmlDecode(GetInnerText(document, "//h2[contains(text(), 'Inriktning och profil')]/following-sibling::p")),
                            InneOchUtemiljo = HtmlDecode(GetInnerText(document, "//h2[contains(text(), 'Inne- och utemiljö')]/following-sibling::p")),
                            KostOchMaltider = HtmlDecode(GetInnerText(document, "//h2[contains(text(), 'Kost och måltider')]/following-sibling::p")),
                            MalOchVision = HtmlDecode(GetInnerText(document, "//h2[contains(text(), 'Mål och vision')]/following-sibling::p")),
                            MerOmOss = HtmlDecode(GetInnerText(document, "//h2[contains(text(), 'Mer om oss')]/following-sibling::p"))
                        };

                        var contacts = GetContacts(document);
                        forskolan.Kontakter = contacts;

                        _context.Forskolans.Add(forskolan);
                        await _context.SaveChangesAsync();

                        success = true;
                        Console.WriteLine($"Successfully saved data for ID: {id} from URL: {url}");
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to fetch data for URL: {url} with status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred for URL: {url}. Exception: {ex.Message}");
                }
            }

            if (!success)
            {
                Console.WriteLine($"Failed to fetch data for ID: {id}");
            }
        }
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

    private List<KontaktInfo> GetContacts(HtmlDocument document)
    {
        var contacts = new List<KontaktInfo>();
        var contactNodes = document.DocumentNode.SelectNodes("//li[p[contains(text(), 'Rektor')] or p[contains(text(), 'Verksamhetschef')] or p[contains(text(), 'Huvudman')] or p[contains(text(), 'Biträdande rektor')] or p[contains(text(), 'Visningsansvarig')] or p[contains(text(), 'Dagbarnvårdare')]]");

        if (contactNodes != null)
        {
            foreach (var node in contactNodes)
            {
                var role = node.SelectSingleNode("p")?.InnerText.Trim();
                var name = node.SelectSingleNode("h3[@class='unit-contact__name']")?.InnerText.Trim();
                var email = node.SelectSingleNode("a[contains(@href, 'mailto:')]")?.InnerText.Trim();
                var phone = node.SelectSingleNode("a[contains(@href, 'tel:')]")?.InnerText.Trim();

                if (!string.IsNullOrEmpty(role) && !string.IsNullOrEmpty(name))
                {
                    contacts.Add(new KontaktInfo
                    {
                        Namn = HtmlDecode(name),
                        Epost = HtmlDecode(email),
                        Telefon = HtmlDecode(phone),
                        Roll = HtmlDecode(role)
                    });
                }
            }
        }

        return contacts;
    }
}
