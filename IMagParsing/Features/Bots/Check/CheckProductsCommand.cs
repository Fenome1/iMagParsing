using MediatR;

namespace IMagParsing.Features.Bots.Check;

public record CheckProductsCommand(long UserId) : IRequest;