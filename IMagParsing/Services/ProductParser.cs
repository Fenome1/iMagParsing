using System;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using IMagParsing.Common.Interfaces;
using IMagParsing.Core.Models;

namespace IMagParsing.Services;

public class ProductParser(HtmlWeb htmlWeb) : IProductParser
{
    public async Task<ProductParsing[]> ParseImagProducts(string url)
    {
        var doc = await htmlWeb.LoadFromWebAsync(url);

        if (doc is null)
            throw new Exception("Ошибка получения страницы");

        var productNodes = doc.DocumentNode.SelectNodes("//div[@class='product_child']");

        if (productNodes is null)
            throw new Exception("Товары не найдены");
        
        var productName = doc.DocumentNode
            .SelectSingleNode("//span[@property='name' and contains(@class, 'post')]").InnerText;

        return productNodes.Select(p => new ProductParsing
        {
            ProductName = productName,
            ColorType = p.GetAttributeValue("data-selectoptions1", null),
            StorageSize = p.GetAttributeValue("data-selectoptions2", null),
            Price = Convert.ToDecimal(p.GetAttributeValue("data-custom_price", null))
            
        }).ToArray();
    }
}