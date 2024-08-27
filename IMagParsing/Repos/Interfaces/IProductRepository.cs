using IMagParsing.Core.Models;

namespace IMagParsing.Common.Interfaces.Repos;

public interface IProductRepository
{
    Task AddProducts(ProductParsing[] products, CancellationToken cancellationToken = default);
    Task Update(ProductParsing[] products, CancellationToken cancellationToken = default);
    Task<ProductParsing[]> Get();
}