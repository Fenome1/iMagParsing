using MediatR;

namespace IMagParsing.Features.Bots.Chart.Steps.Color;

public record SendColorButtonStepCommand(long UserId) : IRequest;