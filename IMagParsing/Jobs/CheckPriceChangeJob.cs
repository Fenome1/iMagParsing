using IMagParsing.Common.Interfaces;
using IMagParsing.Core.Enums;
using Quartz;

namespace IMagParsing.Jobs;

public class CheckPriceChangeJob(IProductService productService) : IJob
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
            .ToList();

        foreach (var newProduct in newProducts)
        {
            var previousProduct = lastProducts
                .FirstOrDefault(p => p.ProductName == newProduct.ProductName
                                     && p.ColorType == newProduct.ColorType
                                     && p.StorageSize == newProduct.StorageSize);

            if (previousProduct is null || previousProduct.Price == newProduct.Price) continue;

            var priceDifference = newProduct.Price - previousProduct.Price;
            var priceChange = priceDifference > 0 ? "increased" : "decreased";

            Console.WriteLine(
                $"Продукт {newProduct.ProductName} ({newProduct.ColorType}, {newProduct.StorageSize}) price {priceChange} from " +
                $"{previousProduct.Price} to {newProduct.Price} by {Math.Abs(priceDifference)}.");
        }

        await productService.ChangeActualProducts();
    }
}