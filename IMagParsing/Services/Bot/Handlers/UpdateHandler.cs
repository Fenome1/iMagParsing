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
        if (update is { Type: UpdateType.Message, Message: not null })
        {
            var message = update.Message;

            if (message.Text.StartsWith("/subscribe", StringComparison.OrdinalIgnoreCase) ||
                message.Text.StartsWith("/unsubscribe", StringComparison.OrdinalIgnoreCase)
                || message.Text.StartsWith("/start", StringComparison.OrdinalIgnoreCase))
            {
                var userId = message.From.Id;
                var username = message.From.Username;

                await userService.AddUserAsync(new User
                {
                    UserId = userId,
                    Username = username
                });

                if (message.Text.StartsWith("/subscribe", StringComparison.OrdinalIgnoreCase) ||
                    message.Text.StartsWith("/start", StringComparison.OrdinalIgnoreCase))
                {
                    await userService.UpdateSubscribeStatusAsync(userId, true);
                    await botClient.SendTextMessageAsync(userId, "Вы подписались на уведомления.",
                        cancellationToken: cancellationToken);
                }
                else if (message.Text.StartsWith("/unsubscribe", StringComparison.OrdinalIgnoreCase))
                {
                    await userService.UpdateSubscribeStatusAsync(userId, false);
                    await botClient.SendTextMessageAsync(userId, "Вы отписались от уведомлений.",
                        cancellationToken: cancellationToken);
                }
            }

            if (message.Text.StartsWith("/check", StringComparison.OrdinalIgnoreCase))
            {
                var userId = message.From.Id;

                var latestProducts = await productService
                    .GetProductsByStatus(ActualStatus.Last);

                var msg = !latestProducts.Any()
                        ? "Актуальные продукты не найдены"
                        : messageBuilder.BuildFormattedProductList(latestProducts);

                await sendHandler.SendMessage(userId, msg);
            }
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"Ошибка прослушки: {exception.Message}");
        return Task.CompletedTask;
    }
}