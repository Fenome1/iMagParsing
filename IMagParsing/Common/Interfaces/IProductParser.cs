using System.Threading.Tasks;
using IMagParsing.Core.Models;

namespace IMagParsing.Common.Interfaces;

public interface IProductParser
{
    Task<ProductParsing[]> ParseImagProducts(string url);
}