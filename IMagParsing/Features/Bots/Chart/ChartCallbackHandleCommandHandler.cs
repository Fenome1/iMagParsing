using IMagParsing.Common.Enums;
using IMagParsing.Features.Bots.Chart.Steps.Color;
using IMagParsing.Features.Bots.Chart.Steps.Model;
using IMagParsing.Features.Bots.Chart.Steps.Storage;
using IMagParsing.Services.Interfaces;
using IMagParsing.TgBot.Handlers.Interfaces;
using IMagParsing.ViewModels;
using MediatR;

namespace IMagParsing.Features.Bots.Chart;

public class ChartCallbackHandleCommandHandler(
    IUserStateService userStateService,
    ISendHandler sendHandler,
    IMediator mediator)
    : IRequestHandler<ChartCallbackHandleCommand>
{
    public async Task Handle(ChartCallbackHandleCommand request, CancellationToken cancellationToken)
    {
        var userState = userStateService.Get(request.UserId);

        if (userState is null || userState.CurrentStep is ChartStep.Complete)
        {
            await mediator.Send(new SendModelButtonStepCommand(request.UserId), cancellationToken);
            return;
        }

        var callbackData = request.CallbackQuery.Data;

        switch (userState.CurrentStep)
        {
            case ChartStep.Model when callbackData.StartsWith("model_"):
                await HandleModelStepAsync(userState, callbackData, cancellationToken);
                break;
            case ChartStep.Storage when callbackData.StartsWith("storage_"):
                await HandleStorageStepAsync(userState, callbackData, cancellationToken);
                break;
            case ChartStep.Color when callbackData.StartsWith("color_"):
                await HandleColorStepAsync(userState, callbackData, cancellationToken);
                break;
        }
    }

    private async Task HandleModelStepAsync(UserState userState, string callbackData,
        CancellationToken cancellationToken)
    {
        var selectedModel = callbackData["model_".Length..];

        userState.ProductInfo.ProductName = selectedModel;
        userState.CurrentStep = ChartStep.Storage;
        userStateService.SaveUserState(userState);

        await sendHandler.SendTextMessage(userState.UserId, $"Вы выбрали модель: {selectedModel}.", cancellationToken);
        await mediator.Send(new SendStorageButtonStepCommand(userState.UserId), cancellationToken);
    }

    private async Task HandleStorageStepAsync(UserState userState, string callbackData,
        CancellationToken cancellationToken)
    {
        var selectedStorage = callbackData["storage_".Length..];
        ;

        userState.ProductInfo.StorageSize = selectedStorage;
        userState.CurrentStep = ChartStep.Color;
        userStateService.SaveUserState(userState);

        await sendHandler.SendTextMessage(userState.UserId, $"Вы выбрали объем: {selectedStorage}.", cancellationToken);
        await mediator.Send(new SendColorButtonStepCommand(userState.UserId), cancellationToken);
    }

    private async Task HandleColorStepAsync(UserState userState, string callbackData,
        CancellationToken cancellationToken)
    {
        var selectedColor = callbackData["color_".Length..];
        ;

        userState.ProductInfo.Color = selectedColor;
        userState.CurrentStep = ChartStep.Complete;
        userStateService.SaveUserState(userState);

        var productInfo = userState.ProductInfo;
        await sendHandler.SendTextMessage(userState.UserId,
            $"{productInfo.ProductName} {productInfo.StorageSize} {productInfo.Color}", cancellationToken);
    }
}