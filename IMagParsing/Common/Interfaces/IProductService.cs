using IMagParsing.Common.Enums;
using IMagParsing.Core.Models;
using IMagParsing.Services;

namespace IMagParsing.Common.Interfaces;

public interface IProductService
{
    Task AddProducts(ProductParsing[] products);
    Task ResetLastDataProducts();
    Task<ProductParsing[]> GetProductsByStatus(ActualStatus actualStatus);
}