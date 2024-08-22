using HtmlAgilityPack;
using IMagParsing.Common.Interfaces;
using IMagParsing.Infrastructure;
using IMagParsing.Jobs;
using IMagParsing.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Listener;

namespace IMagParsing.Common.Config;

public static class Startup
{
    public static void ConfigureQuartz(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            var group = "productProcessingGroup";
            
            var parseJobKey = new JobKey("ParseProductJob", group);
            var checkPriceChangeJobKey = new JobKey("CheckPriceChangeJob", group);

            q.AddJob<ParseProductJob>(opts => opts.WithIdentity(parseJobKey));
            q.AddJob<CheckPriceChangeJob>(opts => opts.WithIdentity(checkPriceChangeJobKey).StoreDurably());

            q.AddTrigger(opts => opts
                .ForJob(parseJobKey)
                .WithIdentity("ParseProductTrigger", group)
                .StartNow()
                .WithSimpleSchedule(x => x.WithRepeatCount(0)));
            
            q.AddTrigger(opts => opts
                .ForJob(checkPriceChangeJobKey)
                .WithIdentity("CheckPriceChangeTrigger", group)
                .StartNow()
                .WithSimpleSchedule(x => x.WithRepeatCount(0)));

            /*q.AddTrigger(opts => opts
                .ForJob(parseJobKey)
                .WithIdentity("ParseProductJob-trigger-daily-12am")
                .WithCronSchedule("0 0 6 * * ?"));*/

            /*q.AddTrigger(opts => opts
                .ForJob(checkPriceChangeJobKey)
                .WithIdentity("CheckPriceChangeJobKey-trigger-daily-12.01am")
                .WithCronSchedule("0 1 6 * * ?"));*/
        });

        services.AddQuartzHostedService(opt => { opt.WaitForJobsToComplete = true; });
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddTransient<IProductParser, ProductParser>();
        services.AddTransient<IProductService, ProductService>();

        services.AddSingleton<HtmlWeb>();
    }

    public static void ConfigureDbContext(this IServiceCollection services, HostBuilderContext context)
    {
        services.AddDbContext<ProductsContext>(options =>
            options.UseNpgsql(context.Configuration
                .GetConnectionString("DefaultConnection")));
    }
}