using IMagParsing.Common.Interfaces.Repos;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace IMagParsing.Features.Users.Commands.Notify;

public class NotifySubscribersCommandHandler(IUserRepository userRepository, ITelegramBotClient botClient)
    : IRequestHandler<NotifySubscribersCommand>
{
    public async Task Handle(NotifySubscribersCommand request, CancellationToken cancellationToken)
    {
        var subscribers = await userRepository.GetSubscribers();

        foreach (var subscriber in subscribers)
            try
            {
                await botClient.SendTextMessageAsync(subscriber.UserId, request.Message,
                    cancellationToken: cancellationToken);
            }
            catch (ApiRequestException ex)
            {
                subscriber.IsSubscribe = false;
                await userRepository.Update(subscriber, cancellationToken);

                Console.WriteLine($"Ошибка при отправке оповещения подписчику " +
                                  $"{subscriber.UserId} ({ex.ErrorCode} - {ex.Message})");
            }
    }
}