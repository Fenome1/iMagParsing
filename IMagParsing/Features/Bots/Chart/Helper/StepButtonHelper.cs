using Telegram.Bot.Types.ReplyMarkups;

namespace IMagParsing.Features.Bots.Chart.Helper;

public static class StepButtonHelper
{
    public static InlineKeyboardMarkup CreateInlineKeyboard<T>(
        IEnumerable<T> items,
        Func<T, string> dataSelector,
        Func<T, string> textSelector)
    {
        var buttons = items
            .Select(item => InlineKeyboardButton.WithCallbackData(textSelector(item), dataSelector(item)))
            .ToArray();

        return new InlineKeyboardMarkup(buttons.Select(b => new[] { b }));
    }
}