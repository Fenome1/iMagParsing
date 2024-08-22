using IMagParsing.Common.Interfaces;
using IMagParsing.Common.Interfaces.Bot;
using IMagParsing.Core.Enums;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = IMagParsing.Core.Models.User;

namespace IMagParsing.Services.Bot.Handlers;

public class UpdateHandler(
    IUserService userService,
    ISendHandler sendHandler,
    IMessageBuilder messageBuilder,
    IProductService productService) : IUpdateHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message != null)
        {
            var message = update.Message;
            var userId = message.From.Id;

            if (message.Text != null) await HandleCommandAsync(botClient, message, userId, cancellationToken);
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"Ошибка прослушки: {exception.Message}");
        return Task.CompletedTask;
    }

    private async Task HandleCommandAsync(ITelegramBotClient botClient, Message message, long userId,
        CancellationToken cancellationToken)
    {
        if (IsSubscriptionCommand(message.Text))
            await HandleSubscriptionAsync(message.Text, userId, cancellationToken);
        else if (message.Text.StartsWith("/check", StringComparison.OrdinalIgnoreCase))
            await HandleCheckCommandAsync(userId, cancellationToken);
    }

    private bool IsSubscriptionCommand(string text)
    {
        return text.StartsWith("/subscribe", StringComparison.OrdinalIgnoreCase) ||
               text.StartsWith("/unsubscribe", StringComparison.OrdinalIgnoreCase) ||
               text.StartsWith("/start", StringComparison.OrdinalIgnoreCase);
    }

    private async Task HandleSubscriptionAsync(string text, long userId, CancellationToken cancellationToken)
    {
        var isSubscribing = text.StartsWith("/subscribe", StringComparison.OrdinalIgnoreCase) ||
                            text.StartsWith("/start", StringComparison.OrdinalIgnoreCase);

        await userService.AddUserAsync(new User(userId));

        await userService.UpdateSubscribeStatusAsync(userId, isSubscribing);

        var responseMessage = isSubscribing
            ? "Вы подписались на уведомления"
            : "Вы отписались от уведомлений";

        await SendMessageAsync(userId, responseMessage, cancellationToken);
    }

    private async Task HandleCheckCommandAsync(long userId, CancellationToken cancellationToken)
    {
        var latestProducts = await productService.GetProductsByStatus(ActualStatus.Last);
        var responseMessage = !latestProducts.Any()
            ? "Актуальные продукты не найдены"
            : messageBuilder.BuildFormattedProductList(latestProducts);

        await SendMessageAsync(userId, responseMessage, cancellationToken);
    }

    private async Task SendMessageAsync(long userId, string message, CancellationToken cancellationToken)
    {
        const int maxMessageLength = 4096;

        if (message.Length <= maxMessageLength)
        {
            await sendHandler.SendMessage(userId, message);
        }
        else
        {
            var messageParts = SplitMessage(message, maxMessageLength);

            foreach (var part in messageParts)
            {
                await sendHandler.SendMessage(userId, part);
                await Task.Delay(500);
            }
        }
    }

    private IEnumerable<string> SplitMessage(string message, int maxLength)
    {
        for (var i = 0; i < message.Length; i += maxLength)
            yield return message.Substring(i, Math.Min(maxLength, message.Length - i));
    }
}