using IMagParsing.Common.Enums;
using MediatR;

namespace IMagParsing.Features.Bots.Subscription;

public record SubscriptionCommand(BotCommand Command, long UserId) : IRequest;