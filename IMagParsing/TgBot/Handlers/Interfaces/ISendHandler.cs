namespace IMagParsing.TgBot.Handlers.Interfaces;

public interface ISendHandler
{
    Task SendMessage(long userId, string message, CancellationToken cancellationToken = default);
}