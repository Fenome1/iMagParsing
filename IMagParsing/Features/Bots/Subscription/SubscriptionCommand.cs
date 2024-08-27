using IMagParsing.Common.Enums;
using MediatR;

namespace IMagParsing.Features.Bots.Subscription;

public record SubscriptionCommand(BotsCommand Command, long UserId) : IRequest;