using IMagParsing.ViewModels;
using MediatR;

namespace IMagParsing.Features.Bots.Chart.Storage;

public record SendStorageButtonStepCommand(long UserId) : IRequest;
