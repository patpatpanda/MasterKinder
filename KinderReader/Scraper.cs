using System.Net;
using HtmlAgilityPack;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using KinderReader.Models;
using MasterKinder.Data;
using MasterKinder.Models;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.IO;
using PdfReader = iTextSharp.text.pdf.PdfReader;
using PdfTextExtractor = iTextSharp.text.pdf.parser.PdfTextExtractor;

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
            try
            {
                var url = $"https://forskola.stockholm/hitta-forskola/forskola/{id}";
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

                    var contacts = document.DocumentNode.SelectNodes("//li[p[contains(text(), 'Rektor')] or p[contains(text(), 'Verksamhetschef')] or p[contains(text(), 'Huvudman')] or p[contains(text(), 'Biträdande rektor')] or p[contains(text(), 'Dagbarnvårdare')] or p[contains(text(), 'Visningsansvarig')]]");

                    if (contacts != null)
                    {
                        forskolan.Kontakter = new List<KontaktInfo>();
                        foreach (var contactNode in contacts)
                        {
                            var role = contactNode.SelectSingleNode(".//p[@class='unit-contact__title epsilon']").InnerText.Trim();
                            var name = contactNode.SelectSingleNode(".//h3[@class='unit-contact__name epsilon']").InnerText.Trim();
                            var email = contactNode.SelectSingleNode(".//a[contains(@href, 'mailto:')]")?.InnerText.Trim();
                            var phone = contactNode.SelectSingleNode(".//a[contains(@href, 'tel:')]")?.InnerText.Trim();

                            var contact = new KontaktInfo
                            {
                                Namn = HtmlDecode(name),
                                Epost = HtmlDecode(email ?? ""),
                                Telefon = HtmlDecode(phone ?? ""),
                                Roll = HtmlDecode(role),
                                Forskolan = forskolan // Associate with the current Forskolan
                            };

                            forskolan.Kontakter.Add(contact);
                        }
                    }

                    _context.Forskolans.Add(forskolan);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Successfully saved data for ID: {id}");
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"ID: {id} not found (404). Skipping.");
                }
                else
                {
                    Console.WriteLine($"Failed to fetch data for ID: {id} with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing ID: {id}, Exception: {ex.Message}");
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
    public async Task<List<string>> ScrapePdfLinksAsync(string url)
    {
        var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync(url);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var pdfLinks = new List<string>();
        var nodes = htmlDocument.DocumentNode.SelectNodes("//a[contains(@href, '.pdf')]");

        if (nodes != null)
        {
            pdfLinks.AddRange(nodes.Select(node => node.GetAttributeValue("href", string.Empty)));
        }

        return pdfLinks;
    }
    public async Task<string> DownloadAndExtractPdfText(string pdfUrl)
    {
        var httpClient = new HttpClient();
        var pdfBytes = await httpClient.GetByteArrayAsync(pdfUrl);
        var pdfText = string.Empty;

        using (var stream = new MemoryStream(pdfBytes))
        using (var reader = new PdfReader(stream))
        {
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                pdfText += PdfTextExtractor.GetTextFromPage(reader, i);
            }
        }

        return pdfText;
    }
}
