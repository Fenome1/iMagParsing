namespace IMagParsing.Bot.Interfaces;

public interface ISendHandler
{
    Task SendMessage(long userId, string message, CancellationToken cancellationToken = default);
}