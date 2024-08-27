using IMagParsing.Common.Interfaces;
using IMagParsing.Common.Interfaces.Bot;
using IMagParsing.Core.Enums;
using IMagParsing.ViewModels;
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
        if (update is { Type: UpdateType.Message, Message: not null })
        {
            var message = update.Message;
            var userId = message.From.Id;

            if (message.Text != null) await HandleCommandAsync(message, userId, cancellationToken);
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"Ошибка прослушки: {exception.Message}");
        return Task.CompletedTask;
    }

    private async Task HandleCommandAsync(Message message, long userId,
        CancellationToken cancellationToken)
    {
        if (IsSubscriptionHandleCommand(message.Text))
            await HandleSubscriptionAsync(message.Text, userId, cancellationToken);
        else if (message.Text.StartsWith("/check", StringComparison.OrdinalIgnoreCase))
            await HandleCheckCommandAsync(userId, cancellationToken);
    }

    private static bool IsSubscriptionHandleCommand(string text)
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

        var groupedProducts = latestProducts
            .GroupBy(p => new { p.ProductName, p.StorageSize })
            .Select(g => new ProductGroup
            {
                ProductName = g.Key.ProductName,
                StorageSize = g.Key.StorageSize,
                Colors = string.Join(", ", g.Select(p => p.ColorType).Distinct()),
                Price = g.First().Price
            })
            .OrderBy(p => p.ProductName)
            .ThenBy(p => p.StorageSize)
            .ToArray();

        var responseMessage = groupedProducts.Length == 0
            ? "Актуальные продукты не найдены"
            : messageBuilder.BuildFormattedProductList(groupedProducts);

        await SendMessageAsync(userId, responseMessage, cancellationToken);
    }

    private async Task SendMessageAsync(long userId, string message,
        CancellationToken cancellationToken)
    {
        await sendHandler.SendMessage(userId, message, cancellationToken);
    }
}