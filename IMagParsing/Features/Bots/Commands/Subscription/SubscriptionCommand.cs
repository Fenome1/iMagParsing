using IMagParsing.Common.Enums;
using MediatR;

namespace IMagParsing.Features.Bots.Commands.Subscription;

public record SubscriptionCommand(BotCommand Command, long UserId) : IRequest;