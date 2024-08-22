using IMagParsing.Common.Interfaces;
using IMagParsing.Common.Interfaces.Bot;
using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace IMagParsing.Services.Bot.Handlers;

public class SendHandler(ITelegramBotClient botClient, IUserService userService) : ISendHandler
{
    public async Task NotifyAsync(string message)
    {
        var subscribers = await userService.GetSubscribeUsers();
        
        foreach (var subscriber in subscribers)
        {
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
    }

    public async Task SendMessage(long userId, string message)
    {
        try
        {
            await botClient.SendTextMessageAsync(userId, message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при отправке сообщения пользователю {userId}: {ex.Message}");
        }
    }
}