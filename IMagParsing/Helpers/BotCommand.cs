using IMagParsing.Common.Enums;

namespace IMagParsing.Helpers;

public static class BotCommand
{
    public static Common.Enums.BotCommand GetCommand(string textCommand)
    {
        return textCommand.ToLower() switch
        {
            var cmd when cmd.StartsWith("/subscribe", StringComparison.OrdinalIgnoreCase) => Common.Enums.BotCommand.Subscribe,
            var cmd when cmd.StartsWith("/unsubscribe", StringComparison.OrdinalIgnoreCase) => Common.Enums.BotCommand.Unsubscribe,
            var cmd when cmd.StartsWith("/start", StringComparison.OrdinalIgnoreCase) => Common.Enums.BotCommand.Start,
            var cmd when cmd.StartsWith("/check", StringComparison.OrdinalIgnoreCase) => Common.Enums.BotCommand.Check,
            _ => Common.Enums.BotCommand.Unknown
        };
    }
}