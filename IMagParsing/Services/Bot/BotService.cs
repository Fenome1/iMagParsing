using IMagParsing.Common.Interfaces.Bot;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace IMagParsing.Services.Bot;

public class BotService(ITelegramBotClient botClient, IUpdateHandler updateHandler) : IBotService
{
    public void Start()
    {
        try
        {
            botClient.StartReceiving(updateHandler,
                new ReceiverOptions());
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