using IMagParsing.Common.Enums;
using IMagParsing.Repos.Interfaces;
using IMagParsing.Services.Interfaces;
using IMagParsing.TgBot.Handlers.Interfaces;
using IMagParsing.ViewModels;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;

namespace IMagParsing.Features.Bots.Commands.Chart.Steps.FirstModel;

public class SendModelButtonStepCommandHandler(
    IProductRepository productRepository,
    IUserStateService userStateService,
    ISendHandler sendHandler) : IRequestHandler<SendModelButtonStepCommand>
{
    public async Task Handle(SendModelButtonStepCommand request, CancellationToken cancellationToken)
    {
        var lastMonthProducts = await productRepository.GetLastMonthAsync();

        var newUserState = new UserState
        {
            UserId = request.UserId,
            CurrentStep = ChartStep.Model,
            LastMonthProducts = lastMonthProducts
        };

        userStateService.SaveUserStateAsync(newUserState);

        var productModels = newUserState
            .LastMonthProducts.Select(p => p.ProductName).Distinct();

        var buttons = productModels
            .Select(model => InlineKeyboardButton.WithCallbackData(model, $"model_{model}"))
            .ToArray();

        var inlineKeyboard = new InlineKeyboardMarkup(buttons.Select(b => new[] { b }));

        await sendHandler.SendTextMessageAsync(request.UserId, "Укажите модель:",
            replyMarkup: inlineKeyboard, cancellationToken: cancellationToken);
    }
}