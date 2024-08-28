using MediatR;

namespace IMagParsing.Features.Bots.MessageHandle;

public record HandleMessageCommand(Telegram.Bot.Types.Message Message) : IRequest;
