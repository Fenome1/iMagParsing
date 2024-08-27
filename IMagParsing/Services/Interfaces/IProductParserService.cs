using IMagParsing.Core.Models;

namespace IMagParsing.Services.Interfaces;

public interface IProductParserService
{
    Task<ProductParsing[]> ParseImagProducts(string url);
}