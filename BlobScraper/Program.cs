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
                    var records = csv.GetRecords<SurveyResponses_2023>()
                                     .Where(r => r.AvserAr == "2023" && r.Forskoleverksamhet != "Fiktiv enhet" && r.Forskoleenhet != "Förskoleenhet saknas" && r.Forskoleverksamhet != "Fiktiv kommunal") // Filtrera för år 2019 och exkludera "fiktiv enhet"
                                     .ToList();

                    context.SurveyResponses2023.AddRange(records);
                    context.SaveChanges();

                    logger.LogInformation($"Sparade {records.Count} poster till databasen för år 20119.");
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



    public class SurveyResponseMap : ClassMap<SurveyResponses_2023>
    {
        public SurveyResponseMap()
        {
            Map(m => m.AvserAr).Name("AvserAr");
            Map(m => m.ResultatkategoriNamn).Name("ResultatkategoriNamn");
            Map(m => m.Stadsdelsnamnd).Name("Stadsdelsnämnd");
            Map(m => m.Forskoleenhet).Name("Förskoleenhet");
            Map(m => m.Forskoleverksamhet).Name("Förskoleverksamhet");
            Map(m => m.RegiformNamn).Name("RegiformNamn");
            Map(m => m.FrageomradeNr).Name("FrågeområdeNr");
            Map(m => m.Frageomradestext).Name("Frågeområdestext");
            Map(m => m.FrageNr).Name("FrågeNr");
            Map(m => m.Fragetext).Name("Frågetext");
            Map(m => m.Kortfragetext).Name("Kortfrågetext");
            Map(m => m.SvarsalternativTyp).Name("SvarsalternativTyp");
            Map(m => m.Fragetyp).Name("Frågetyp");
            Map(m => m.Fragekategori).Name("Frågekategori");
            Map(m => m.SvarsalternativNr).Name("SvarsalternativNr");
            Map(m => m.SvarsalternativText).Name("SvarsalternativText");
            // Lägg till mappning för alla övriga fält
            Map(m => m.ResultatkategoriKod).Name("ResultatkategoriKod");
            Map(m => m.Organisatoriskenhetskod).Name("Organisatoriskenhetskod");
            Map(m => m.AntalSvarsalternativ).Name("AntalSvarsalternativ");
            Map(m => m.GraderingSvarsalternativ).Name("GraderingSvarsalternativ");
            Map(m => m.Enkatroll).Name("Enkätroll");
            Map(m => m.Respondentroll).Name("Respondentroll");
            Map(m => m.Kon).Name("Kön");
            Map(m => m.Utfall).Name("Utfall");
            Map(m => m.TotalVarde).Name("TotalVarde");
            Map(m => m.TotalVarde_ExklVetEj).Name("TotalVarde_ExklVetEj");
        }
    }

}
