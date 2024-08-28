using IMagParsing.TgBot.Handlers.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IMagParsing.TgBot.Handlers;

public class SendHandler(ITelegramBotClient botClient) : ISendHandler
{
    private const int MaxMessageLength = 4096;

    public async Task SendTextMessage(long userId, string message, CancellationToken cancellationToken = default,
        IReplyMarkup? replyMarkup = null)
    {
        try
        {
            if (message.Length <= MaxMessageLength)
            {
                await botClient.SendTextMessageAsync(userId, message,
                    replyMarkup: replyMarkup,
                    cancellationToken: cancellationToken);
            }
            else
            {
                var messageParts = SplitMessage(message, MaxMessageLength);

                foreach (var part in messageParts)
                {
                    await botClient.SendTextMessageAsync(userId, part,
                        cancellationToken: cancellationToken);
                    await Task.Delay(500, cancellationToken);
                }
            }
        }
        catch (ApiRequestException ex)
        {
            Console.WriteLine($"Ошибка при отправки сообщения пользователю: " +
                              $"{userId} ({ex.ErrorCode} - {ex.Message})");
        }
    }

    public async Task AnswerCallbackQuery(string callbackQueryId, string? text = null, bool showAlert = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId,
                text,
                showAlert,
                cancellationToken: cancellationToken
            );
        }
        catch (ApiRequestException ex)
        {
            Console.WriteLine($"Ошибка при ответе на CallbackQuery: {callbackQueryId} ({ex.ErrorCode} - {ex.Message})");
        }
    }

    public async Task SendImage(long chatId, byte[] imageBytes, string caption = null, CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream(imageBytes);
        var inputMedia = new InputFileStream(stream, $"chart_for_{chatId}.png");

        try
        {
            await botClient.SendPhotoAsync(
                chatId: chatId,
                photo: inputMedia,
                caption: caption,
                cancellationToken: cancellationToken
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending photo: {ex.Message}");
        }
    }

    private static IEnumerable<string> SplitMessage(string message, int maxLength)
    {
        for (var i = 0; i < message.Length; i += maxLength)
            yield return message.Substring(i, Math.Min(maxLength,
                message.Length - i));
    }
}