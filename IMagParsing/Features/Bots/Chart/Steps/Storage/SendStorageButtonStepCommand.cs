using MediatR;

namespace IMagParsing.Features.Bots.Chart.Steps.Storage;

public record SendStorageButtonStepCommand(long UserId) : IRequest;