using MediatR;

namespace IMagParsing.Features.Bots.Commands.Chart.Steps.SecondStorage;

public record SendStorageButtonStepCommand(long UserId) : IRequest;