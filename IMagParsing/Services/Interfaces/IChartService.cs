using IMagParsing.Core.Models;
using IMagParsing.ViewModels;

namespace IMagParsing.Services.Interfaces;

public interface IChartService
{
    byte[] GeneratePriceTrendChartAsync(UserState userState);
}