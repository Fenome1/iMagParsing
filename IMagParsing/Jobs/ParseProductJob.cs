using IMagParsing.Common.Config;
using IMagParsing.Common.Interfaces.Services;
using IMagParsing.Features.Products.Commands.Add;
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
                await mediator.Send(new AddParsingProductsCommand(
                    await parserService.ParseImagProducts(url)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
    }
}