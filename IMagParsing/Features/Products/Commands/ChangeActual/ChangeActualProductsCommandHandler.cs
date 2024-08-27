using IMagParsing.Core.Enums;
using IMagParsing.Repos.Interfaces;
using MediatR;

namespace IMagParsing.Features.Products.Commands.ChangeActual;

public class ChangeActualProductsCommandHandler(IProductRepository productRepository)
    : IRequestHandler<ChangeActualProductsCommand>
{
    public async Task Handle(ChangeActualProductsCommand request, CancellationToken cancellationToken)
    {
        var allProducts = await productRepository.Get();

        foreach (var product in allProducts)
            if (product.ActualStatus is ActualStatus.New or ActualStatus.Last)
                product.ActualStatus--;

        await productRepository.Update(allProducts, cancellationToken);
    }
}