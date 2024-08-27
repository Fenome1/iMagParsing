using Telegram.Bot.Types.ReplyMarkups;

namespace IMagParsing.TgBot.Handlers.Interfaces;

public interface ISendHandler
{
    Task SendTextMessage(long userId, string message, CancellationToken cancellationToken = default, IReplyMarkup? replyMarkup = null);
}