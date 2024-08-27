using IMagParsing.ViewModels;

namespace IMagParsing.Common.Interfaces.Services;

public interface IMessageService
{
    string BuildPriceChangeMessage(PriceChangedProduct[] products);
    string BuildFormattedProductList(ProductGroup[] products);
}