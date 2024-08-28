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

            var message = $"📱 Название: {product.ProductName}\n" +
                          $"🎨 Цвет: {product.ColorType}\n" +
                          $"💿 Размер: {product.StorageSize}.\n" +
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
            .Select(p => $"📱 Название: {p.ProductName} \n" +
                         $"🎨 Цвет(-а): {p.Colors}\n" +
                         $"💿 Размер: {p.StorageSize}.\n" +
                         $"💵 Цена: {p.Price.FormatPrice()} Руб.\n")
            .ToList();

        return string.Join("\n", formattedList);
    }
}