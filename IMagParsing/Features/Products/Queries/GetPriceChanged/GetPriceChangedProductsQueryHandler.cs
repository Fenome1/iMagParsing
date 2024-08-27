using IMagParsing.Core.Enums;
using IMagParsing.Features.Products.Commands.ChangeActual;
using IMagParsing.Features.Products.Queries.GetByStatus;
using IMagParsing.ViewModels;
using MediatR;

namespace IMagParsing.Features.Products.Queries.GetPriceChanged;

public class GetPriceChangedProductsQueryHandler(IMediator mediator)
    : IRequestHandler<GetPriceChangedProductsQuery, PriceChangedProduct[]>
{
    public async Task<PriceChangedProduct[]> Handle(GetPriceChangedProductsQuery request,
        CancellationToken cancellationToken)
    {
        var lastProducts = await mediator.Send(new GetProductsByStatusQuery(ActualStatus.Last), cancellationToken);

        if (lastProducts.Length == 0)
        {
            await mediator.Send(new ChangeActualProductsCommand(), cancellationToken);
            return [];
        }

        var newProducts = (await mediator.Send(new GetProductsByStatusQuery(ActualStatus.New), cancellationToken))
            .OrderBy(p => p.ProductName)
            .ThenBy(p => p.StorageSize)
            .ThenBy(p => p.Price)
            .ToArray();

        var changedProducts = newProducts
            .Select(newProduct =>
            {
                var previousProduct = lastProducts
                    .FirstOrDefault(p => p.ProductName == newProduct.ProductName
                                         && p.ColorType == newProduct.ColorType
                                         && p.StorageSize == newProduct.StorageSize);

                if (previousProduct == null || previousProduct.Price == newProduct.Price)
                    return null;

                var priceDifference = newProduct.Price - previousProduct.Price;

                return new PriceChangedProduct
                {
                    ProductName = newProduct.ProductName,
                    ColorType = newProduct.ColorType,
                    StorageSize = newProduct.StorageSize,
                    OldPrice = previousProduct.Price,
                    CurrentPrice = newProduct.Price,
                    Deference = Math.Abs(priceDifference),
                    IsPriceUp = priceDifference > 0
                };
            })
            .Where(product => product is not null)
            .ToArray();

        await mediator.Send(new ChangeActualProductsCommand(), cancellationToken);

        return changedProducts!;
    }
}