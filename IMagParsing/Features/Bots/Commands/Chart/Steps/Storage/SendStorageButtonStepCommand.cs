using MediatR;

namespace IMagParsing.Features.Bots.Commands.Chart.Steps.Storage;

public record SendStorageButtonStepCommand(long UserId) : IRequest;