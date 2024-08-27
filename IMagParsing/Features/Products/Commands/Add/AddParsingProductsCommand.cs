using IMagParsing.Core.Models;
using MediatR;

namespace IMagParsing.Features.Products.Commands.Add;

public record AddParsingProductsCommand(ProductParsing[] Products) : IRequest;