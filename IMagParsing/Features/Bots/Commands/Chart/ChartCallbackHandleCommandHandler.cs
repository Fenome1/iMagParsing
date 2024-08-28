using IMagParsing.Common.Enums;
using IMagParsing.Features.Bots.Commands.Chart.Steps.FirstModel;
using IMagParsing.Features.Bots.Commands.Chart.Steps.SecondStorage;
using IMagParsing.Features.Bots.Commands.Chart.Steps.ThirdColor;
using IMagParsing.Features.Users.Commands.SendChart;
using IMagParsing.Services.Interfaces;
using IMagParsing.TgBot.Handlers.Interfaces;
using IMagParsing.ViewModels;
using MediatR;

namespace IMagParsing.Features.Bots.Commands.Chart;

public class ChartCallbackHandleCommandHandler(
    IUserStateService userStateService,
    ISendHandler sendHandler,
    IMediator mediator)
    : IRequestHandler<ChartCallbackHandleCommand>
{
    public async Task Handle(ChartCallbackHandleCommand request, CancellationToken cancellationToken)
    {
        var userState = await userStateService.GetAsync(request.UserId);

        if (userState is null || userState.CurrentStep is ChartStep.Complete)
        {
            await mediator.Send(new SendModelButtonStepCommand(request.UserId), cancellationToken);
            await sendHandler.AnswerCallbackQueryAsync(request.CallbackQuery.Id, cancellationToken: cancellationToken);
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

        await sendHandler.AnswerCallbackQueryAsync(request.CallbackQuery.Id, cancellationToken: cancellationToken);
    }

    private async Task HandleModelStepAsync(UserState userState, string callbackData,
        CancellationToken cancellationToken)
    {
        var selectedModel = callbackData["model_".Length..];

        userState.ProductInfo.ProductName = selectedModel;
        userState.CurrentStep = ChartStep.Storage;
        await userStateService.SaveUserStateAsync(userState);

        await sendHandler.SendTextMessageAsync(userState.UserId, $"Вы выбрали модель: {selectedModel}.",
            cancellationToken);
        await mediator.Send(new SendStorageButtonStepCommand(userState.UserId), cancellationToken);
    }

    private async Task HandleStorageStepAsync(UserState userState, string callbackData,
        CancellationToken cancellationToken)
    {
        var selectedStorage = callbackData["storage_".Length..];

        userState.ProductInfo.StorageSize = selectedStorage;
        userState.CurrentStep = ChartStep.Color;
        await userStateService.SaveUserStateAsync(userState);

        await sendHandler.SendTextMessageAsync(userState.UserId, $"Вы выбрали объем: {selectedStorage}.",
            cancellationToken);
        await mediator.Send(new SendColorButtonStepCommand(userState.UserId), cancellationToken);
    }

    private async Task HandleColorStepAsync(UserState userState, string callbackData,
        CancellationToken cancellationToken)
    {
        var selectedColor = callbackData["color_".Length..];

        userState.ProductInfo.Color = selectedColor;
        userState.CurrentStep = ChartStep.Complete;
        await userStateService.SaveUserStateAsync(userState);
        
        await sendHandler.SendTextMessageAsync(userState.UserId, 
            "Генерация графика, пожалуйста, подождите...", 
            cancellationToken: cancellationToken);

        await mediator.Send(new SendChartCommand(userState.UserId), cancellationToken);

        await userStateService.DeleteUserStateAsync(userState.UserId);
    }
}