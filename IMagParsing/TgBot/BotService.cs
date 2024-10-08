﻿using Telegram.Bot;
using Telegram.Bot.Polling;

namespace IMagParsing.TgBot;

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
            Console.WriteLine(e.Message);
        }
    }
}