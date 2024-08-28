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

namespace IMagParsing.TgBot.Handlers
{
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
                        await HandleMessageAsync(update.Message, cancellationToken);
                        break;
                    }

                    case { Type: UpdateType.CallbackQuery, CallbackQuery: not null }:
                    {
                        await HandleCallbackQueryAsync(update.CallbackQuery, cancellationToken);
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

        private async Task HandleMessageAsync(Message message, CancellationToken cancellationToken)
        {
            var userId = message.From.Id;

            if (message.Text != null)
            {
                var command = BotCommand.GetCommand(message.Text);

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

        private async Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            await mediator.Send(new ChartCallbackHandleCommand(callbackQuery.From.Id, callbackQuery),
                cancellationToken);
        }
    }
}