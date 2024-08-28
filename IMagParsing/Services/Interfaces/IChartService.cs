using IMagParsing.ViewModels;

namespace IMagParsing.Services.Interfaces;

public interface IChartService
{
    Task<byte[]> GeneratePriceTrendChart(UserState userState, CancellationToken cancellationToken = default);
}