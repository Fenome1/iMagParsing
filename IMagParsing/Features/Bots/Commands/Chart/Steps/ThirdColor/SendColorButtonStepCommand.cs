using MediatR;

namespace IMagParsing.Features.Bots.Commands.Chart.Steps.ThirdColor;

public record SendColorButtonStepCommand(long UserId) : IRequest;