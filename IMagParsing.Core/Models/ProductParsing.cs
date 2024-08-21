using System;

namespace IMagParsing.Core.Models;

public class ProductParsing
{
    public int ProductParsingId { get; set; }
    public string ProductName { get; set; }
    public string ColorType { get; set; }
    public string StorageSize { get; set; }
    public decimal Price { get; set; }
    public DateTime ParsingDate { get; set; }
}