using IMagParsing.Common;
using IMagParsing.Common.Interfaces;
using IMagParsing.Common.Interfaces.Bot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((config) => { config.AddJsonFile("appsettings.json", false, true); })
    .ConfigureServices((context, services) =>
    {
        services.ConfigureDbContext(context);
        services.ConfigureServices(context.Configuration);
        services.ConfigureQuartz();
    }).Build();

var botService = host.Services.GetRequiredService<IBotService>();
botService.Start();

await host.RunAsync();