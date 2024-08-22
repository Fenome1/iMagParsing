using System.Globalization;
using IMagParsing.Common.Interfaces;
using IMagParsing.Core.Models;

namespace IMagParsing.Services
{
    public class MessageBuilder : IMessageBuilder
    {
        public string BuildPriceChangeMessage(ProductParsing[] lastProducts, ProductParsing[] newProducts)
        {
            var priceChangeMessages = new List<string>();

            foreach (var newProduct in newProducts)
            {
                var previousProduct = lastProducts
                    .FirstOrDefault(p => p.ProductName == newProduct.ProductName
                                         && p.ColorType == newProduct.ColorType
                                         && p.StorageSize == newProduct.StorageSize);

                if (previousProduct == null || previousProduct.Price == newProduct.Price)
                    continue;

                var priceDifference = newProduct.Price - previousProduct.Price;
                var priceChange = priceDifference > 0 ? "увеличилась \u2b06\ufe0f" : "уменьшилась \u2b07\ufe0f";
                var absPriceDifference = Math.Abs(priceDifference);

                var message = $"Название: {newProduct.ProductName}\n" +
                              $"Цвет: {newProduct.ColorType}\n" +
                              $"Размер: {newProduct.StorageSize}\n" +
                              $"Старая цена: {FormatPrice(previousProduct.Price)}\n" +
                              $"Новая цена: {FormatPrice(newProduct.Price)}\n" +
                              $"{priceChange} на {FormatPrice(absPriceDifference)}\n";

                priceChangeMessages.Add(message);
            }
            
            return string.Join("\n", priceChangeMessages);
        }

        public string BuildFormattedProductList(ProductParsing[] products)
        {
            var formattedList = products
                .OrderBy(p => p.ProductName)
                .Select(p => $"Название: {p.ProductName}\n" +
                             $"Цвет: {p.ColorType}\n" +
                             $"Размер: {p.StorageSize}\n" +
                             $"Цена: {FormatPrice(p.Price)} руб.\n")
                .ToList();

            return string.Join("\n", formattedList);
        }

        private string FormatPrice(decimal price)
        {
            return price.ToString("N2", CultureInfo.InvariantCulture);
        }
    }
}
