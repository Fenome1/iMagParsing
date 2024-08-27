using IMagParsing.Common;
using IMagParsing.Infrastructure;
using IMagParsing.TgBot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config => { config.AddJsonFile("appsettings.json", false, true); })
    .ConfigureServices((context, services) =>
    {
        services.ConfigureDbContext(context);
        services.ConfigureServices(context.Configuration);
        services.ConfigureQuartz();
    }).Build();

var context = host.Services.GetService<ProductsContext>();
await context?.Database.MigrateAsync();

var botService = host.Services.GetRequiredService<IBotService>();
botService.Start();

await host.RunAsync();