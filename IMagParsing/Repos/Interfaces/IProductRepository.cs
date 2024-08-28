using IMagParsing.Core.Models;

namespace IMagParsing.Repos.Interfaces;

public interface IProductRepository
{
    Task AddProductsAsync(ProductParsing[] products, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProductParsing[] products, CancellationToken cancellationToken = default);
    Task<ProductParsing[]> GetAsync();
    Task<ProductParsing[]> GetLastMonthAsync();
}