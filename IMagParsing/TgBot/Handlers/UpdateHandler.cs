using IMagParsing.Features.Bots.Chart;
using IMagParsing.Features.Bots.Chart.Steps.Model;
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
        switch (update)
        {
            case { Type: UpdateType.Message, Message: not null }:
            {
                var message = update.Message;
                var userId = message.From.Id;

                if (message.Text != null)
                    await HandleCommandAsync(message, userId, cancellationToken);
                break;
            }
            case { Type: UpdateType.CallbackQuery, CallbackQuery: not null }:
            {
                var userId = update.CallbackQuery.From.Id;
                await mediator.Send(new ChartCallbackHandleCommand(userId, update.CallbackQuery),
                    cancellationToken);
                break;
            }
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
            case Common.Enums.BotCommand.Subscribe:
            case Common.Enums.BotCommand.Unsubscribe:
            case Common.Enums.BotCommand.Start:
                await mediator.Send(new SubscriptionCommand(command, userId), cancellationToken);
                break;
            case Common.Enums.BotCommand.Check:
                await mediator.Send(new CheckProductsCommand(userId), cancellationToken);
                break;
            case Common.Enums.BotCommand.Chart:
                await mediator.Send(new SendModelButtonStepCommand(userId), cancellationToken);
                break;
        }
    }
}