using Telegram.Bot.Types.ReplyMarkups;

namespace IMagParsing.TgBot.Handlers.Interfaces;

public interface ISendHandler
{
    Task SendTextMessageAsync(long userId, string message, CancellationToken cancellationToken = default,
        IReplyMarkup? replyMarkup = null);

    Task AnswerCallbackQueryAsync(string callbackQueryId, string? text = null, bool showAlert = false,
        CancellationToken cancellationToken = default);

    Task SendImageAsync(long userId, byte[] imageBytes, string caption = null,
        CancellationToken cancellationToken = default);
}