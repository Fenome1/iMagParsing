using IMagParsing.Common.Interfaces;
using IMagParsing.Core.Enums;
using IMagParsing.Core.Models;
using IMagParsing.Infrastructure;
using IMagParsing.Infrastructure.Common.Extentions;
using Microsoft.EntityFrameworkCore;

namespace IMagParsing.Services;

public class ProductService(ProductsContext context) : IProductService
{
    public async Task AddProducts(ProductParsing[] products)
    {
        await context.WithTransactionAsync(async () =>
        {
            foreach (var productParsing in products)
                productParsing.ActualStatus = ActualStatus.New;

            await context.ProductParsings.AddRangeAsync(products);
            await context.SaveChangesAsync();
        });
    }

    public async Task ChangeActualProducts()
    {
        await context.WithTransactionAsync(async () =>
        {
            var allProducts = await context.ProductParsings.ToListAsync();

            foreach (var product in allProducts)
                if (product.ActualStatus is ActualStatus.New or ActualStatus.Last)
                    product.ActualStatus--;

            await context.SaveChangesAsync();
        });
    }

    public async Task<ProductParsing[]> GetProductsByStatus(ActualStatus actualStatus)
    {
        return await context.ProductParsings
            .Where(p => p.ActualStatus == actualStatus)
            .ToArrayAsync();
    }
}