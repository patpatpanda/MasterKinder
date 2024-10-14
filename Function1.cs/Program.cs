using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = new HostBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables();  // Lägg till miljövariabler från Azure Portal vid behov
    })
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureLogging(logging =>
    {
        logging.AddConsole();  // Lägg till logging till konsolen
        logging.SetMinimumLevel(LogLevel.Information);  // Sätt lägsta loggnivå
    })
    .Build();

host.Run();
