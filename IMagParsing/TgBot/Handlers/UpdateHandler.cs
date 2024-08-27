using IMagParsing.Common.Enums;
using IMagParsing.Features.Bots.Check;
using IMagParsing.Features.Bots.Subscription;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using BotCommand = IMagParsing.Helpers.BotCommand;

namespace IMagParsing.TgBot.Handlers;

public class UpdateHandler(IMediator mediator) : IUpdateHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update is { Type: UpdateType.Message, Message: not null })
        {
            var message = update.Message;
            var userId = message.From.Id;

            if (message.Text != null)
                await HandleCommandAsync(message, userId, cancellationToken);
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"Ошибка прослушки: {exception.Message}");
        return Task.CompletedTask;
    }

    private async Task HandleCommandAsync(Message message, long userId, CancellationToken cancellationToken)
    {
        var command = BotCommand.GetCommand(message.Text!);

        switch (command)
        {
            case BotsCommand.Subscribe:
            case BotsCommand.Unsubscribe:
            case BotsCommand.Start:
                await mediator.Send(new SubscriptionCommand(command, userId), cancellationToken);
                break;
            case BotsCommand.Check:
                await mediator.Send(new CheckProductsCommand(userId), cancellationToken);
                break;
        }
    }
}