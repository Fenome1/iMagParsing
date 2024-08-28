using MediatR;

namespace IMagParsing.Features.Bots.Chart.Steps.Model;

public record SendModelButtonStepCommand(long UserId) : IRequest;