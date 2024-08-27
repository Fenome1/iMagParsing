using MediatR;

namespace IMagParsing.Features.Bots.Chart.Model;

public record SendModelButtonStepCommand(long UserId) : IRequest;
