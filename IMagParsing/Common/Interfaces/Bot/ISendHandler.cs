namespace IMagParsing.Common.Interfaces.Bot;

public interface ISendHandler
{
    Task NotifyAsync(string message);
    Task SendMessage(long userId, string message);
}