using IMagParsing.Common.Interfaces;
using IMagParsing.Common.Interfaces.Bot;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace IMagParsing.Services;

public class BotService(ITelegramBotClient botClient, IUpdateHandler updateHandler) : IBotService
{
    public void Start()
    {
        try
        {
            botClient.StartReceiving(updateHandler,
                new ReceiverOptions(),
                cancellationToken: default);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void Stop()
    {
    }
}