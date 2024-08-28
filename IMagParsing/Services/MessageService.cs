using IMagParsing.Helpers;
using IMagParsing.Services.Interfaces;
using IMagParsing.ViewModels;

namespace IMagParsing.Services;

public class MessageService : IMessageService
{
    public string BuildPriceChangeMessage(PriceChangedProduct[] products)
    {
        var priceChangeMessages = new List<string>();

        foreach (var product in products)
        {
            var priceChange = product.IsPriceUp ? "⬆ Увеличилась" : "⬇ Уменьшилась";

            var message = $"📱 Название: {product.ProductName} {product.StorageSize}\n" +
                          $"🎨 Цвет: {product.ColorType}\n" +
                          $"💸 Старая цена: {product.OldPrice.FormatPrice()} Руб.\n" +
                          $"💵 Новая цена: {product.CurrentPrice.FormatPrice()} Руб.\n" +
                          $"{priceChange} на {product.Deference.FormatPrice()} Руб.\n";

            priceChangeMessages.Add(message);
        }

        return string.Join("\n", priceChangeMessages);
    }

    public string BuildFormattedProductList(ProductGroup[] products)
    {
        var formattedList = products
            .OrderBy(p => p.ProductName)
            .ThenBy(p => p.StorageSize.ExtractStorageSize())
            .Select(p =>
            {
                var price = p.Price;
                var formattedPrice = price > 0 ? $"{price.FormatPrice()} Руб." : "по запросу";

                return $"📱 Название: {p.ProductName} {p.StorageSize}\n" +
                       $"💵 Цена: {formattedPrice} \n" +
                       $"🎨 Цвет(-а): {p.Colors}\n";
            })
            .ToList();

        return string.Join("\n", formattedList);
    }
}