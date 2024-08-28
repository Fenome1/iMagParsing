using MediatR;

namespace IMagParsing.Features.Bots.Commands.Chart.Steps.Color;

public record SendColorButtonStepCommand(long UserId) : IRequest;