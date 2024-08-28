using IMagParsing.Core.Models;
using IMagParsing.Infrastructure;
using IMagParsing.Infrastructure.Common.Extentions;
using IMagParsing.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMagParsing.Repos;

public class ProductRepository(ProductsContext context) : IProductRepository
{
    public async Task AddProducts(ProductParsing[] products, CancellationToken cancellationToken)
    {
        await context.WithTransactionAsync(async () =>
        {
            await context.ProductParsings.AddRangeAsync(products);
            await context.SaveChangesAsync(cancellationToken);
        }, cancellationToken);
    }

    public async Task Update(ProductParsing[] products, CancellationToken cancellationToken)
    {
        await context.WithTransactionAsync(async () =>
        {
            context.ProductParsings.UpdateRange(products);
            await context.SaveChangesAsync(cancellationToken);
        }, cancellationToken);
    }

    public async Task<ProductParsing[]> Get()
    {
        return await context.ProductParsings
            .ToArrayAsync();
    }

    public async Task<ProductParsing[]> GetLastMonth()
    {
        return await context.ProductParsings
            .Where(p => p.ParsingDate >= DateTime.UtcNow.AddMonths(-1))
            .ToArrayAsync();
    }
}