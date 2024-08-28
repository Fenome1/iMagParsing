using IMagParsing.Core.Models;
using IMagParsing.Infrastructure;
using IMagParsing.Infrastructure.Common.Extentions;
using IMagParsing.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMagParsing.Repos;

public class ProductRepository(ProductsContext context) : IProductRepository
{
    public async Task AddProductsAsync(ProductParsing[] products, CancellationToken cancellationToken)
    {
        await context.WithTransactionAsync(async () =>
        {
            await context.ProductParsings.AddRangeAsync(products);
            await context.SaveChangesAsync(cancellationToken);
        }, cancellationToken);
    }

    public async Task UpdateAsync(ProductParsing[] products, CancellationToken cancellationToken)
    {
        await context.WithTransactionAsync(async () =>
        {
            context.ProductParsings.UpdateRange(products);
            await context.SaveChangesAsync(cancellationToken);
        }, cancellationToken);
    }

    public async Task<ProductParsing[]> GetAsync()
    {
        return await context.ProductParsings
            .ToArrayAsync();
    }

    public async Task<ProductParsing[]> GetLastMonthAsync()
    {
        return await context.ProductParsings
            .Where(p => p.ParsingDate >= DateTime.UtcNow.AddMonths(-1))
            .ToArrayAsync();
    }
}