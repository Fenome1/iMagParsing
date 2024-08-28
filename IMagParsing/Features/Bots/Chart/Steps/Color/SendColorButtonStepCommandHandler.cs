using IMagParsing.Services.Interfaces;
using IMagParsing.TgBot.Handlers.Interfaces;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;

namespace IMagParsing.Features.Bots.Chart.Steps.Color;

public class SendColorButtonStepCommandHandler(
    ISendHandler sendHandler,
    IUserStateService userStateService) : IRequestHandler<SendColorButtonStepCommand>
{
    public async Task Handle(SendColorButtonStepCommand request, CancellationToken cancellationToken)
    {
        var userState = await userStateService.Get(request.UserId);

        var productColors = userState.LastMonthProducts
            .Where(p => p.ProductName == userState.ProductInfo.ProductName)
            .Where(p => p.StorageSize == userState.ProductInfo.StorageSize)
            .Select(p => p.ColorType)
            .Distinct();

        var buttons = productColors
            .Select(color => InlineKeyboardButton.WithCallbackData(color, $"color_{color}"))
            .ToArray();

        var inlineKeyboard = new InlineKeyboardMarkup(buttons.Select(b => new[] { b }));

        await sendHandler.SendTextMessage(request.UserId, "Укажите необходимый цвет:",
            replyMarkup: inlineKeyboard, cancellationToken: cancellationToken);
    }
}