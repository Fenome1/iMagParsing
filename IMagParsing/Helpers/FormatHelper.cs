using System.Globalization;

namespace IMagParsing.Helpers;

public static class FormatHelper
{
    public static string FormatPrice(this decimal price)
    {
        return price
            .ToString("N2", CultureInfo.InvariantCulture)
            .Replace(",", " ");
    }

    public static int ExtractStorageSize(this string storageSize)
    {
        var sizeDigits = new string(storageSize.Where(char.IsDigit).ToArray());
        return int.TryParse(sizeDigits, out var size) ? size : 0;
    }
}