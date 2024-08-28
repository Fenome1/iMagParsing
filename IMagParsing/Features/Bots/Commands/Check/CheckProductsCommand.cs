using MediatR;

namespace IMagParsing.Features.Bots.Commands.Check;

public record CheckProductsCommand(long UserId) : IRequest;