using IMagParsing.Common.Enums;
using IMagParsing.Core.Models;
using IMagParsing.Repos.Interfaces;
using IMagParsing.TgBot.Handlers.Interfaces;
using MediatR;

namespace IMagParsing.Features.Bots.Commands.Subscription;

public class SubscriptionCommandHandler(IUserRepository userRepository, ISendHandler sendHandler)
    : IRequestHandler<SubscriptionCommand>
{
    public async Task Handle(SubscriptionCommand request, CancellationToken cancellationToken)
    {
        var isSubscribing = request.Command is BotCommand.Start or BotCommand.Subscribe;

        var user = await userRepository.GetUserAsync(request.UserId);

        if (user is null)
        {
            await userRepository.AddUserAsync(new User(request.UserId), cancellationToken);
        }
        else
        {
            if (user.IsSubscribe == isSubscribing)
            {
                var alreadySubscribedMessage = isSubscribing
                    ? "Вы уже подписаны на уведомления."
                    : "Вы уже отписаны от уведомлений.";

                await sendHandler.SendTextMessageAsync(request.UserId, alreadySubscribedMessage, cancellationToken);
                return;
            }

            user.IsSubscribe = isSubscribing;
            await userRepository.UpdateAsync(user, cancellationToken);
        }

        var responseMessage = isSubscribing
            ? "Вы подписались на уведомления"
            : "Вы отписались от уведомлений";

        await sendHandler.SendTextMessageAsync(request.UserId, responseMessage, cancellationToken);
    }
}