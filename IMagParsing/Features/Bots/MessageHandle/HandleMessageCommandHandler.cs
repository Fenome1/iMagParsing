using IMagParsing.Features.Bots.Commands.Chart.Steps.FirstModel;
using IMagParsing.Features.Bots.Commands.Check;
using IMagParsing.Features.Bots.Commands.Subscription;
using IMagParsing.Helpers;
using MediatR;

namespace IMagParsing.Features.Bots.MessageHandle;

public class HandleMessageCommandHandler(IMediator mediator) : IRequestHandler<HandleMessageCommand>
{
    public async Task Handle(HandleMessageCommand request, CancellationToken cancellationToken)
    {
        var userId = request.Message.From.Id;

        if (request.Message.Text != null)
        {
            var command = BotCommand.GetCommand(request.Message.Text);

            switch (command)
            {
                case Common.Enums.BotCommand.Subscribe:
                case Common.Enums.BotCommand.Unsubscribe:
                case Common.Enums.BotCommand.Start:
                    await mediator.Send(new SubscriptionCommand(command, userId), cancellationToken);
                    break;

                case Common.Enums.BotCommand.Check:
                    await mediator.Send(new CheckProductsCommand(userId), cancellationToken);
                    break;

                case Common.Enums.BotCommand.Chart:
                    await mediator.Send(new SendModelButtonStepCommand(userId), cancellationToken);
                    break;
            }
        }
    }
}