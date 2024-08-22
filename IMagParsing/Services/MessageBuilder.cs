using System.Globalization;
using IMagParsing.Common.Interfaces;
using IMagParsing.Core.Models;
using IMagParsing.ViewModels;

namespace IMagParsing.Services;

public class MessageBuilder : IMessageBuilder
{
    public string BuildPriceChangeMessage(PriceChangedProduct[] products)
    {
        var priceChangeMessages = new List<string>();

        foreach (var product in products)
        {
            var priceChange = product.IsPriceUp ? "⬆ Увеличилась" : "⬇ Уменьшилась";

            var message = $"📱 Название: {product.ProductName}\n" +
                          $"🎨 Цвет: {product.ColorType}\n" +
                          $"💿 Размер: {product.StorageSize}.\n\n" +
                          $"💸 Старая цена: {FormatPrice(product.OldPrice)} Руб.\n" +
                          $"💵 Новая цена: {FormatPrice(product.CurrentPrice)} Руб.\n" +
                          $"{priceChange} на {FormatPrice(product.Deference)} Руб.\n";

            priceChangeMessages.Add(message);
        }

        return string.Join("\n", priceChangeMessages);
    }

    public string BuildFormattedProductList(ProductParsing[] products)
    {
        var formattedList = products
            .OrderByDescending(p => p.ProductName)
            .Select(p => $"📱 Название: {p.ProductName} \n" +
                         $"🎨 Цвет: {p.ColorType}\n" +
                         $"💿 Размер: {p.StorageSize}.\n" +
                         $"💵 Цена: {FormatPrice(p.Price)} Руб.\n")
            .ToList();

        return string.Join("\n", formattedList);
    }

    private string FormatPrice(decimal price)
    {
        return price
            .ToString("N2", CultureInfo.InvariantCulture)
            .Replace(",", " ");
    }
}