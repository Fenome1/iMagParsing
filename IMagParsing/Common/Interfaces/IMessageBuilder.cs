using IMagParsing.ViewModels;

namespace IMagParsing.Common.Interfaces;

public interface IMessageBuilder
{
    string BuildPriceChangeMessage(PriceChangedProduct[] products);
    string BuildFormattedProductList(ProductGroup[] products);
}