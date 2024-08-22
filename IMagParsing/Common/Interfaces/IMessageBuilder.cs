using IMagParsing.Core.Models;

namespace IMagParsing.Common.Interfaces;

public interface IMessageBuilder
{
    string BuildPriceChangeMessage(ProductParsing[] lastProducts, ProductParsing[] newProducts);
    string BuildFormattedProductList(ProductParsing[] products);
}