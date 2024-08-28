using MediatR;

namespace IMagParsing.Features.Bots.Commands.Chart.Steps.Model;

public record SendModelButtonStepCommand(long UserId) : IRequest;