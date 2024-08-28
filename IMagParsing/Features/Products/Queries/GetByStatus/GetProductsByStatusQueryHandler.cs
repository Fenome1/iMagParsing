using IMagParsing.Core.Models;
using IMagParsing.Repos.Interfaces;
using MediatR;

namespace IMagParsing.Features.Products.Queries.GetByStatus;

public class GetProductsByStatusQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductsByStatusQuery, ProductParsing[]>
{
    public async Task<ProductParsing[]> Handle(GetProductsByStatusQuery request, CancellationToken cancellationToken)
    {
        return (await productRepository.GetAsync())
            .Where(p => p.ActualStatus == request.Status)
            .ToArray();
    }
}