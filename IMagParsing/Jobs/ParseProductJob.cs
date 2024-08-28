using IMagParsing.Common.Config;
using IMagParsing.Features.Products.Commands.Add;
using IMagParsing.Services.Interfaces;
using MediatR;
using Quartz;

namespace IMagParsing.Jobs;

public class ParseProductJob(IProductParserService parserService, IMediator mediator) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        foreach (var url in UrlDataConfig.ParsingUrls)
            try
            {
                var products = await parserService.ParseImagProductsAsync(url);
                await mediator.Send(new AddParsingProductsCommand(products));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
    }
}