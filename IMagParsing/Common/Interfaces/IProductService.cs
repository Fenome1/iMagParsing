using IMagParsing.Core.Enums;
using IMagParsing.Core.Models;

namespace IMagParsing.Common.Interfaces;

public interface IProductService
{
    Task AddProducts(ProductParsing[] products);
    Task ChangeActualProducts();
    Task<ProductParsing[]> GetProductsByStatus(ActualStatus actualStatus);
}