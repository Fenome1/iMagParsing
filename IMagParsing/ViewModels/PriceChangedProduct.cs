namespace IMagParsing.ViewModels;

public class PriceChangedProduct
{
    public string ProductName { get; set; }
    public string ColorType { get; set; }
    public string StorageSize { get; set; }
    public decimal OldPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal Deference { get; set; }
    public bool IsPriceUp { get; set; }
}