using MediatR;

namespace IMagParsing.Features.Users.Commands.Notify;

public record NotifySubscribersCommand(string Message) : IRequest;