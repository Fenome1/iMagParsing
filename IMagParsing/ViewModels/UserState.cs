using IMagParsing.Common.Enums;
using IMagParsing.Core.Models;

namespace IMagParsing.ViewModels;

public class UserState
{
    public long UserId { get; set; }
    public ChartStep CurrentStep { get; set; }
    public ProductInfo ProductInfo { get; set; } = new();
    public ProductParsing[]? LastMonthProducts { get; set; }
}