using IMagParsing.Common.Interfaces;
using IMagParsing.Core.Enums;
using IMagParsing.Core.Models;
using IMagParsing.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IMagParsing.Services;

public class ProductService(ProductsContext context) : IProductService
{
    public async Task AddProducts(ProductParsing[] products)
    {
        await context.Database.BeginTransactionAsync();

        foreach (var productParsing in products)
            productParsing.ActualStatus = ActualStatus.New;

        await context.ProductParsings.AddRangeAsync(products);
        await context.SaveChangesAsync();

        await context.Database.CommitTransactionAsync();
    }

    public async Task ResetLastDataProducts()
    {
        await context.Database.BeginTransactionAsync();

        var allProducts = await context.ProductParsings.ToListAsync();

        foreach (var product in allProducts)
            if (product.ActualStatus is ActualStatus.New or ActualStatus.Last)
                product.ActualStatus--;

        await context.SaveChangesAsync();

        await context.Database.CommitTransactionAsync();
    }

    public async Task<ProductParsing[]> GetProductsByStatus(ActualStatus actualStatus)
    {
        return await context.ProductParsings
            .Where(p => p.ActualStatus == actualStatus)
            .ToArrayAsync();
    }
}