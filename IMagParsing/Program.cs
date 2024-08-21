using IMagParsing.Common.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) => { config.AddJsonFile("appsettings.json", false, true); })
    .ConfigureServices((context, services) =>
    {
        services.ConfigureDbContext(context);

        services.ConfigureServices();
        services.ConfigureQuartz();
    }).Build();

await host.RunAsync();