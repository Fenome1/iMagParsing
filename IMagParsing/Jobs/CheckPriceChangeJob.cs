using IMagParsing.Common.Interfaces.Services;
using IMagParsing.Features.Products.Queries.GetPriceChanged;
using IMagParsing.Features.Users.Commands.Notify;
using MediatR;
using Quartz;

namespace IMagParsing.Jobs;

public class CheckPriceChangeJob(IMediator mediator, IMessageService messageService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var changedProducts = await mediator.Send(new GetPriceChangedProductsQuery());

            if (changedProducts.Length is 0)
                return;

            var message = messageService.BuildPriceChangeMessage(changedProducts);

            if (!string.IsNullOrWhiteSpace(message))
                await mediator.Send(new NotifySubscribersCommand(message));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}