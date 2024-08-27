using IMagParsing.Core.Models;

namespace IMagParsing.Common.Interfaces.Services;

public interface IProductParserService
{
    Task<ProductParsing[]> ParseImagProducts(string url);
}