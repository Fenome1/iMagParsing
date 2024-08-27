using HtmlAgilityPack;
using IMagParsing.Common.Config;
using IMagParsing.Infrastructure;
using IMagParsing.Jobs;
using IMagParsing.Repos;
using IMagParsing.Repos.Interfaces;
using IMagParsing.Services;
using IMagParsing.Services.Interfaces;
using IMagParsing.TgBot;
using IMagParsing.TgBot.Handlers;
using IMagParsing.TgBot.Handlers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Quartz;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace IMagParsing.Common;

public static class Startup
{
    public static void ConfigureQuartz(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            var parseJobKey = new JobKey("ParseProductJob");
            var checkPriceChangeJobKey = new JobKey("CheckPriceChangeJob");

            q.AddJob<ParseProductJob>(opts => opts.WithIdentity(parseJobKey));
            q.AddJob<CheckPriceChangeJob>(opts => opts.WithIdentity(checkPriceChangeJobKey));

            q.AddTrigger(opts => opts
                .ForJob(parseJobKey)
                .WithIdentity("ParseProductJob-trigger")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(30)
                    .RepeatForever()));

            q.AddTrigger(opts => opts
                .ForJob(checkPriceChangeJobKey)
                .WithIdentity("CheckPriceChangeJob-trigger")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(30)
                    .RepeatForever())
                .StartAt(DateBuilder.FutureDate(5, IntervalUnit.Second)));
        });

        services.AddQuartzHostedService(opt => { opt.WaitForJobsToComplete = true; });
    }

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IProductParserService, ProductParserService>();
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IMessageService, MessageService>();

        services.AddSingleton<HtmlWeb>();

        services.ConfigureTelegramBot(configuration);

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));
    }

    public static void ConfigureDbContext(this IServiceCollection services, HostBuilderContext context)
    {
        services.AddDbContext<ProductsContext>(options =>
            options.UseNpgsql(context.Configuration
                .GetConnectionString("DefaultConnection")));
    }

    private static void ConfigureTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BotConfig>(configuration.GetSection("BotConfig"));

        services.AddSingleton<ITelegramBotClient>(p =>
        {
            var botConfig = p.GetRequiredService<IOptions<BotConfig>>().Value;
            return new TelegramBotClient(botConfig.Token);
        });

        services.AddTransient<IUpdateHandler, UpdateHandler>();
        services.AddTransient<ISendHandler, SendHandler>();

        services.AddTransient<IBotService, BotService>();
    }
}