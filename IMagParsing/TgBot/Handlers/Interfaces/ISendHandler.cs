using Telegram.Bot.Types.ReplyMarkups;

namespace IMagParsing.TgBot.Handlers.Interfaces;

public interface ISendHandler
{
    Task SendTextMessage(long userId, string message, CancellationToken cancellationToken = default,
        IReplyMarkup? replyMarkup = null);

    Task AnswerCallbackQuery(string callbackQueryId, string? text = null, bool showAlert = false,
        CancellationToken cancellationToken = default);

    Task SendImage(long userId, byte[] imageBytes, string caption = null,
        CancellationToken cancellationToken = default);
}