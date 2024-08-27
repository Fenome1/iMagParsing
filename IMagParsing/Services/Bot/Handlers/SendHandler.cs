using IMagParsing.Common.Interfaces;
using IMagParsing.Common.Interfaces.Bot;
using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace IMagParsing.Services.Bot.Handlers;

public class SendHandler(ITelegramBotClient botClient, IUserService userService) : ISendHandler
{
    private const int MaxMessageLength = 4096;

    public async Task NotifyAsync(string message)
    {
        var subscribers = await userService.GetSubscribeUsers();

        foreach (var subscriber in subscribers)
            try
            {
                await botClient.SendTextMessageAsync(subscriber.UserId, message);
            }
            catch (ApiRequestException ex) when (ex.Message.Contains("bot was blocked by the user"))
            {
                await userService.UpdateSubscribeStatusAsync(subscriber.UserId, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке сообщения пользователю {subscriber.UserId}: {ex.Message}");
            }
    }

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
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при отправке сообщения пользователю {userId}: {ex.Message}");
        }
    }

    private static IEnumerable<string> SplitMessage(string message, int maxLength)
    {
        for (var i = 0; i < message.Length; i += maxLength)
            yield return message.Substring(i, Math.Min(maxLength,
                message.Length - i));
    }
}