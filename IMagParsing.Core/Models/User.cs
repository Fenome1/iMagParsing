namespace IMagParsing.Core.Models;

public class User(long userId)
{
    public long UserId { get; set; } = userId;
    public bool IsSubscribe { get; set; }
}