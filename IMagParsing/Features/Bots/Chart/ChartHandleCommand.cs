using MediatR;
using Telegram.Bot.Types;

namespace IMagParsing.Features.Bots.Chart;

public record ChartHandleCommand(long UserId, CallbackQuery CallbackQuery) : IRequest;
