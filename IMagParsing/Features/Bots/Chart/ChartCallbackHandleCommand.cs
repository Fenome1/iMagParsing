using MediatR;
using Telegram.Bot.Types;

namespace IMagParsing.Features.Bots.Chart;

public record ChartCallbackHandleCommand(long UserId, CallbackQuery CallbackQuery) : IRequest;