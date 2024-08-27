using IMagParsing.Core.Enums;
using IMagParsing.Core.Models;
using MediatR;

namespace IMagParsing.Features.Products.Queries.GetByStatus;

public record GetProductsByStatusQuery(ActualStatus Status) : IRequest<ProductParsing[]>;