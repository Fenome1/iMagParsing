using System;
using System.Linq;
using System.Threading.Tasks;
using IMagParsing.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace IMagParsing.Jobs;

public class CheckPriceChangeJob(ProductsContext dbContext) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var previousProducts = await dbContext.ProductParsings
            .Where(p => p.ParsingDate.Date < DateTime.UtcNow.Date)
            .ToListAsync();
        
        var latestProducts = await dbContext.ProductParsings
            .Where(p => p.ParsingDate.Date == DateTime.UtcNow.Date)
            .OrderBy(p => p.ProductName)
            .ThenBy(p => p.StorageSize)
            .ThenBy(p => p.Price)
            .ToListAsync();
        
        foreach (var latestProduct in latestProducts)
        {
            var previousProduct = previousProducts
                .FirstOrDefault(p => p.ProductName == latestProduct.ProductName 
                                     && p.ColorType == latestProduct.ColorType 
                                     && p.StorageSize == latestProduct.StorageSize);

            if (previousProduct is not null && previousProduct.Price != latestProduct.Price)
            {
                var priceDifference = latestProduct.Price - previousProduct.Price;
                var priceChange = priceDifference > 0 ? "increased" : "decreased";
    
                Console.WriteLine($"Продукт {latestProduct.ProductName} ({latestProduct.ColorType}, {latestProduct.StorageSize}) price {priceChange} from " +
                                  $"{previousProduct.Price} to {latestProduct.Price} by {Math.Abs(priceDifference)}.");
            }
        }
    }
}