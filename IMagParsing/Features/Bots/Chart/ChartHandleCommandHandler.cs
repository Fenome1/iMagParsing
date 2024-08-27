using IMagParsing.Common.Enums;
using IMagParsing.Features.Bots.Chart.Model;
using IMagParsing.Features.Bots.Chart.Storage;
using IMagParsing.Services.Interfaces;
using IMagParsing.TgBot.Handlers.Interfaces;
using MediatR;

namespace IMagParsing.Features.Bots.Chart;

public class ChartHandleCommandHandler(IUserStateService userStateService, ISendHandler sendHandler, IMediator mediator)
    : IRequestHandler<ChartHandleCommand>
{
    public async Task Handle(ChartHandleCommand request, CancellationToken cancellationToken)
    {
        var userState = userStateService.Get(request.UserId);
        
        if (userState is null)
        {
            await mediator.Send(new SendModelButtonStepCommand(request.UserId), cancellationToken);
            return;
        }
        
        var callbackData = request.CallbackQuery.Data;
        
        if (userState.CurrentStep == ChartStep.Model && callbackData.StartsWith("model_"))
        {
            var selectedModel = callbackData["model_".Length..];

            userState.ProductInfo.ProductName = selectedModel;
            userState.CurrentStep = ChartStep.Store;

            userStateService.SaveUserState(userState);

            await sendHandler.SendTextMessage(request.UserId, $"Вы выбрали модель: {selectedModel}. Теперь выберите объем памяти:",
                cancellationToken: cancellationToken);
            
            await mediator.Send(new SendStorageButtonStepCommand(request.UserId), cancellationToken);
        }
    }
}