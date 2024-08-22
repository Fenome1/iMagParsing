using IMagParsing.Common.Interfaces;
using IMagParsing.Common.Interfaces.Bot;
using IMagParsing.Core.Enums;
using IMagParsing.ViewModels;
using Quartz;

namespace IMagParsing.Jobs;

public class CheckPriceChangeJob(
    IProductService productService,
    IMessageBuilder messageBuilder,
    ISendHandler sendHandler) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var changedProducts = await GetPriceChangedProducts();

            if (!changedProducts.Any())
                return;

            var message = messageBuilder.BuildPriceChangeMessage(changedProducts);

            if (!string.IsNullOrWhiteSpace(message))
                await sendHandler.NotifyAsync(message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task<PriceChangedProduct[]> GetPriceChangedProducts()
    {
        var lastProducts = await productService.GetProductsByStatus(ActualStatus.Last);

        if (!lastProducts.Any())
        {
            await productService.ChangeActualProducts();
            return [];
        }

        var newProducts = (await productService.GetProductsByStatus(ActualStatus.New))
            .OrderBy(p => p.ProductName)
            .ThenBy(p => p.StorageSize)
            .ThenBy(p => p.Price)
            .ToArray();

        var changedProducts = newProducts
            .Select(newProduct =>
            {
                var previousProduct = lastProducts
                    .FirstOrDefault(p => p.ProductName == newProduct.ProductName
                                         && p.ColorType == newProduct.ColorType
                                         && p.StorageSize == newProduct.StorageSize);

                if (previousProduct == null || previousProduct.Price == newProduct.Price)
                    return null;

                var priceDifference = newProduct.Price - previousProduct.Price;

                return new PriceChangedProduct
                {
                    ProductName = newProduct.ProductName,
                    ColorType = newProduct.ColorType,
                    StorageSize = newProduct.StorageSize,
                    OldPrice = previousProduct.Price,
                    CurrentPrice = newProduct.Price,
                    Deference = Math.Abs(priceDifference),
                    IsPriceUp = priceDifference > 0
                };
            })
            .Where(product => product != null)
            .ToArray();

        await productService.ChangeActualProducts();

        return changedProducts;
    }
}