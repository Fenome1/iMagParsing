using IMagParsing.Common.Enums;

namespace IMagParsing.Helpers;

public static class BotCommand
{
    public static BotsCommand GetCommand(string textCommand)
    {
        return textCommand.ToLower() switch
        {
            var cmd when cmd.StartsWith("/subscribe", StringComparison.OrdinalIgnoreCase) => BotsCommand.Subscribe,
            var cmd when cmd.StartsWith("/unsubscribe", StringComparison.OrdinalIgnoreCase) => BotsCommand.Unsubscribe,
            var cmd when cmd.StartsWith("/start", StringComparison.OrdinalIgnoreCase) => BotsCommand.Start,
            var cmd when cmd.StartsWith("/check", StringComparison.OrdinalIgnoreCase) => BotsCommand.Check,
            _ => BotsCommand.Unknown
        };
    }
}