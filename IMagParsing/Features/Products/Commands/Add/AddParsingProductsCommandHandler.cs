using IMagParsing.Common.Interfaces.Repos;
using IMagParsing.Core.Enums;
using MediatR;

namespace IMagParsing.Features.Products.Commands.Add;

public class AddParsingProductsCommandHandler(IProductRepository productRepository)
    : IRequestHandler<AddParsingProductsCommand>
{
    public async Task Handle(AddParsingProductsCommand request, CancellationToken cancellationToken)
    {
        foreach (var productParsing in request.Products)
            productParsing.ActualStatus = ActualStatus.New;

        await productRepository.AddProducts(request.Products, cancellationToken);
    }
}