using MediatR;

namespace IMagParsing.Features.Bots.Commands.Chart.Steps.FirstModel;

public record SendModelButtonStepCommand(long UserId) : IRequest;