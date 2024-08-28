using IMagParsing.Core.Enums;
using IMagParsing.Repos.Interfaces;
using MediatR;

namespace IMagParsing.Features.Products.Commands.Add;

public class AddParsingProductsCommandHandler(IProductRepository productRepository)
    : IRequestHandler<AddParsingProductsCommand>
{
    public async Task Handle(AddParsingProductsCommand request, CancellationToken cancellationToken)
    {
        foreach (var productParsing in request.Products)
            productParsing.ActualStatus = ActualStatus.New;

        await productRepository.AddProductsAsync(request.Products, cancellationToken);
    }
}