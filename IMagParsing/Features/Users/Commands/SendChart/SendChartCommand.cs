using MediatR;

namespace IMagParsing.Features.Users.Commands.SendChart;

public record SendChartCommand(long UserId) : IRequest;
