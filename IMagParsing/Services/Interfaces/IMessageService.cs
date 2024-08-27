using IMagParsing.ViewModels;

namespace IMagParsing.Services.Interfaces;

public interface IMessageService
{
    string BuildPriceChangeMessage(PriceChangedProduct[] products);
    string BuildFormattedProductList(ProductGroup[] products);
}