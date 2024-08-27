using IMagParsing.ViewModels;
using MediatR;

namespace IMagParsing.Features.Products.Queries.GetPriceChanged;

public class GetPriceChangedProductsQuery : IRequest<PriceChangedProduct[]>;