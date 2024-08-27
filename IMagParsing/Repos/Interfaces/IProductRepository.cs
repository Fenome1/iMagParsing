using IMagParsing.Core.Models;

namespace IMagParsing.Repos.Interfaces;

public interface IProductRepository
{
    Task AddProducts(ProductParsing[] products, CancellationToken cancellationToken = default);
    Task Update(ProductParsing[] products, CancellationToken cancellationToken = default);
    Task<ProductParsing[]> Get();
    Task<ProductParsing[]> GetLastMonth();
}