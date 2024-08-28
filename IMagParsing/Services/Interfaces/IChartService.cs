using IMagParsing.ViewModels;

namespace IMagParsing.Services.Interfaces;

public interface IChartService
{
    Task<byte[]> GenerateChartAsync(UserState userState, CancellationToken cancellationToken = default);
}