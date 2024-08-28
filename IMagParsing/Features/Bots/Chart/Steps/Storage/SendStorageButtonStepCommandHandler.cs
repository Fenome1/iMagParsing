using IMagParsing.Helpers;
using IMagParsing.Services.Interfaces;
using IMagParsing.TgBot.Handlers.Interfaces;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;

namespace IMagParsing.Features.Bots.Chart.Steps.Storage;

public class SendStorageButtonStepCommandHandler(ISendHandler sendHandler, IUserStateService userStateService)
    : IRequestHandler<SendStorageButtonStepCommand>
{
    public async Task Handle(SendStorageButtonStepCommand request, CancellationToken cancellationToken)
    {
        var userState = await userStateService.Get(request.UserId);

        var productStorages = userState.LastMonthProducts
            .Where(p => p.ProductName == userState.ProductInfo.ProductName)
            .OrderBy(p => p.StorageSize.ExtractStorageSize())
            .Select(p => p.StorageSize)
            .Distinct();

        var buttons = productStorages
            .Select(storage => InlineKeyboardButton.WithCallbackData(storage, $"storage_{storage}"))
            .ToArray();

        var inlineKeyboard = new InlineKeyboardMarkup(buttons.Select(b => new[] { b }));

        await sendHandler.SendTextMessage(request.UserId, "Укажите объем памяти:",
            replyMarkup: inlineKeyboard, cancellationToken: cancellationToken);
    }
}