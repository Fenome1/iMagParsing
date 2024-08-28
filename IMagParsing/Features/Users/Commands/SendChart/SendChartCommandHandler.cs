using IMagParsing.Services.Interfaces;
using IMagParsing.TgBot.Handlers.Interfaces;
using MediatR;

namespace IMagParsing.Features.Users.Commands.SendChart;

public class SendChartCommandHandler(
    ISendHandler sendHandler,
    IUserStateService stateService,
    IChartService chartService) : IRequestHandler<SendChartCommand>
{
    public async Task Handle(SendChartCommand request, CancellationToken cancellationToken)
    {
        var userState = await stateService.Get(request.UserId);

        if (userState is null)
            throw new Exception($"Ошибка получения состояния пользователя: {request.UserId}");

        var bytes = await chartService.GeneratePriceTrendChart(userState, cancellationToken);

        await sendHandler.SendImage(request.UserId, bytes, cancellationToken: cancellationToken);
    }
}