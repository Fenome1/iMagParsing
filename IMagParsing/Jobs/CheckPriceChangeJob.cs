using IMagParsing.Common.Interfaces;
using IMagParsing.Common.Interfaces.Bot;
using IMagParsing.Core.Enums;
using Quartz;

namespace IMagParsing.Jobs;

public class CheckPriceChangeJob(
    IProductService productService,
    IMessageBuilder messageBuilder,
    ISendHandler sendHandler) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var lastProducts = await productService.GetProductsByStatus(ActualStatus.Last);

        if (!lastProducts.Any())
        {
            await productService.ChangeActualProducts();
            return;
        }

        var newProducts = (await productService.GetProductsByStatus(ActualStatus.New))
            .OrderBy(p => p.ProductName)
            .ThenBy(p => p.StorageSize)
            .ThenBy(p => p.Price)
            .ToArray();

        var priceChangeMessage = messageBuilder.BuildPriceChangeMessage(lastProducts, newProducts);

        if (!string.IsNullOrWhiteSpace(priceChangeMessage))
            await sendHandler.NotifyAsync(priceChangeMessage);
            
        await productService.ChangeActualProducts();
    }
}