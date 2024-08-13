using System;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Azure.Storage.Blobs;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MasterKinder.Models;
using MasterKinder.Data;
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<MrDb>();
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                // Logga att vi börjar hämta filen
                logger.LogInformation("Startar nedladdning av CSV-filen från Azure Blob Storage.");

                // Azure Blob Storage connection string
                string blobConnectionString = configuration["BlobConnectionString"];
                string blobContainerName = "dagis-data";
                string blobFileName = "UND_FSK.csv";

                var blobServiceClient = new BlobServiceClient(blobConnectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
                var blobClient = containerClient.GetBlobClient(blobFileName);

                // Ladda ner blob-filen till en stream
                MemoryStream memoryStream = new MemoryStream();
                blobClient.DownloadTo(memoryStream);
                memoryStream.Position = 0;

                logger.LogInformation("Nedladdning av CSV-fil slutförd.");

                // Registrera kodningsleverantören för att stödja äldre teckenkodningar som Windows-1252
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding sourceEncoding = Encoding.GetEncoding("Windows-1252");

                // CsvHelper-konfiguration
                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Encoding = sourceEncoding,
                    Delimiter = ",",
                    HasHeaderRecord = true,
                };

                logger.LogInformation("Börjar läsa och bearbeta data från CSV-filen.");

                using (var reader = new StreamReader(memoryStream, sourceEncoding))
                using (var csv = new CsvReader(reader, csvConfig))
                {
                    csv.Context.RegisterClassMap<SurveyResponseMap>(); // Registrera din anpassade mappning
                    var records = csv.GetRecords<SurveyResponse>()
                                     .Where(r => r.AvserAr == "2023" && r.Forskoleverksamhet != "Fiktiv enhet" && r.Forskoleenhet != "Förskoleenhet saknas" && r.Forskoleverksamhet != "Fiktiv kommunal") // Filtrera för år 2019 och exkludera "fiktiv enhet"
                                     .ToList();

                    context.SurveyResponses.AddRange(records);
                    context.SaveChanges();

                    logger.LogInformation($"Sparade {records.Count} poster till databasen för år 2019.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ett fel inträffade under bearbetningen.");
            }
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<MrDb>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DefaultSQLConnection")));

                services.AddSingleton<IConfiguration>(context.Configuration);

                // Lägg till loggningstjänster
                services.AddLogging(configure =>
                {
                    configure.AddConsole();
                    configure.AddDebug();
                });
            });



    public class SurveyResponseMap : ClassMap<SurveyResponse>
    {
        public SurveyResponseMap()
        {
            Map(m => m.AvserAr).Name("AvserAr");
            Map(m => m.ResultatkategoriNamn).Name("ResultatkategoriNamn");
            Map(m => m.Stadsdelsnamnd).Name("Stadsdelsnämnd");
            Map(m => m.Forskoleenhet).Name("Förskoleenhet");
            Map(m => m.Forskoleverksamhet).Name("Förskoleverksamhet");
            Map(m => m.RegiformNamn).Name("RegiformNamn");
            Map(m => m.FragaomradeNr).Name("FrågeområdeNr");
            Map(m => m.Fragaomradestext).Name("Frågeområdestext");
            Map(m => m.FragaNr).Name("FrågeNr");
            Map(m => m.Fragetext).Name("Frågetext");
            Map(m => m.Kortfragetext).Name("Kortfrågetext");
            Map(m => m.SvarsalternativTyp).Name("SvarsalternativTyp");
            Map(m => m.Fragetyp).Name("Frågetyp");
            Map(m => m.Fragkategori).Name("Frågekategori");
            Map(m => m.SvarsalternativNr).Name("SvarsalternativNr");
            Map(m => m.SvarsalternativText).Name("SvarsalternativText");
        }
    }

}
