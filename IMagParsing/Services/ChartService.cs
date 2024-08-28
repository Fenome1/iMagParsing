using IMagParsing.Services.Interfaces;
using IMagParsing.ViewModels;
using ScottPlot;

namespace IMagParsing.Services;

public class ChartService : IChartService
{
    public async Task<byte[]> GenerateChartAsync(UserState userState, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            var productInfo = userState.ProductInfo;

            var groupedData = userState.LastMonthProducts
                .Where(p => p.ProductName == productInfo.ProductName &&
                            p.StorageSize == productInfo.StorageSize &&
                            p.ColorType == productInfo.Color)
                .OrderBy(p => p.ParsingDate.Date)
                .GroupBy(p => p.ParsingDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    AveragePrice = g.Average(p => p.Price)
                })
                .ToArray();

            var xs = groupedData.Select(g => g.Date.ToOADate()).ToArray();
            var ys = groupedData.Select(g => (double)g.AveragePrice).ToArray();

            var plt = new Plot();

            var sp = plt.Add.Scatter(xs, ys);

            sp.Smooth = true;
            sp.LineWidth = 3;
            sp.MarkerSize = 5;

            plt.Axes.DateTimeTicksBottom();

            plt.Title($"{productInfo.ProductName} {productInfo.StorageSize} ({productInfo.Color})");

            plt.YLabel("Цена");
            plt.XLabel("Дата");

            return plt.GetImageBytes(600, 400);
        }, cancellationToken);
    }
}