using IMagParsing.Common.Interfaces.Repos;
using IMagParsing.Core.Models;
using MediatR;

namespace IMagParsing.Features.Products.Queries.GetByStatus;

public class GetProductsByStatusQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductsByStatusQuery, ProductParsing[]>
{
    public async Task<ProductParsing[]> Handle(GetProductsByStatusQuery request, CancellationToken cancellationToken)
    {
        return (await productRepository.Get())
            .Where(p => p.ActualStatus == request.Status)
            .ToArray();
    }
}