using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = new HostBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables();  // L�gg till milj�variabler fr�n Azure Portal vid behov
    })
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureLogging(logging =>
    {
        logging.AddConsole();  // L�gg till logging till konsolen
        logging.SetMinimumLevel(LogLevel.Information);  // S�tt l�gsta loggniv�
    })
    .Build();

host.Run();
