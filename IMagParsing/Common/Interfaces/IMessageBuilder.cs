using IMagParsing.Core.Models;
using IMagParsing.ViewModels;

namespace IMagParsing.Common.Interfaces;

public interface IMessageBuilder
{
    string BuildPriceChangeMessage(PriceChangedProduct[] products);
    string BuildFormattedProductList(ProductParsing[] products);
}