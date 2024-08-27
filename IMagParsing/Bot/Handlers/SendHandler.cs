using IMagParsing.Bot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace IMagParsing.Bot.Handlers;

public class SendHandler(ITelegramBotClient botClient) : ISendHandler
{
    private const int MaxMessageLength = 4096;

    public async Task SendMessage(long userId, string message, CancellationToken cancellationToken)
    {
        try
        {
            if (message.Length <= MaxMessageLength)
            {
                await botClient.SendTextMessageAsync(userId, message,
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

    private static IEnumerable<string> SplitMessage(string message, int maxLength)
    {
        for (var i = 0; i < message.Length; i += maxLength)
            yield return message.Substring(i, Math.Min(maxLength,
                message.Length - i));
    }
}