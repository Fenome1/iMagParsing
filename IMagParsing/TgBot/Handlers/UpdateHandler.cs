using IMagParsing.Features.Bots.Chart;
using IMagParsing.Features.Bots.Chart.Steps.Model;
using IMagParsing.Features.Bots.Check;
using IMagParsing.Features.Bots.Message;
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
    public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            switch (update)
            {
                case { Type: UpdateType.Message, Message: not null }:
                {
                    await mediator.Send(new HandleMessageCommand(update.Message), cancellationToken);
                    break;
                }

                case { Type: UpdateType.CallbackQuery, CallbackQuery: not null }:
                {
                    var callbackQuery = update.CallbackQuery;
                    await mediator.Send(new ChartCallbackHandleCommand
                        (callbackQuery.From.Id, callbackQuery), cancellationToken);
                    break;
                }
            }
        }, cancellationToken);

        return Task.CompletedTask;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        Task.Run(() => { Console.WriteLine($"Ошибка прослушки: {exception.Message}"); }, cancellationToken);
        return Task.CompletedTask;
    }
}