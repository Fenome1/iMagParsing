using MediatR;

namespace IMagParsing.Features.Bots.Message;

public record HandleMessageCommand(Telegram.Bot.Types.Message Message) : IRequest;
