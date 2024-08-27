using IMagParsing.Core.Enums;
using IMagParsing.Features.Products.Queries.GetByStatus;
using IMagParsing.Repos.Interfaces;
using IMagParsing.Services.Interfaces;
using IMagParsing.TgBot.Handlers.Interfaces;
using IMagParsing.ViewModels;
using MediatR;

namespace IMagParsing.Features.Bots.Check;

public class CheckProductsCommandHandler(
    IProductRepository productRepository,
    ISendHandler sendHandler,
    IMessageService messageService,
    IMediator mediator) : IRequestHandler<CheckProductsCommand>
{
    public async Task Handle(CheckProductsCommand request, CancellationToken cancellationToken)
    {
        var latestProducts =
            await mediator.Send(new GetProductsByStatusQuery(ActualStatus.Last), cancellationToken);

        var groupedProducts = latestProducts
            .GroupBy(p => new { p.ProductName, p.StorageSize })
            .Select(g => new ProductGroup
            {
                ProductName = g.Key.ProductName,
                StorageSize = g.Key.StorageSize,
                Colors = string.Join(", ", g.Select(p => p.ColorType).Distinct()),
                Price = g.First().Price
            })
            .OrderBy(p => p.ProductName)
            .ThenBy(p => p.StorageSize)
            .ToArray();

        var responseMessage = groupedProducts.Length == 0
            ? "Актуальные продукты не найдены"
            : messageService.BuildFormattedProductList(groupedProducts);

        await sendHandler.SendTextMessage(request.UserId, responseMessage, cancellationToken);
    }
}