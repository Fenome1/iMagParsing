using IMagParsing.Common.Config;
using IMagParsing.Common.Interfaces;
using Quartz;

namespace IMagParsing.Jobs;

public class ParseProductJob(IProductParser parser, IProductService productService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        foreach (var url in UrlData.ParsingUrls)
            try
            {
                var products = await parser.ParseImagProducts(url);
                await productService.AddProducts(products);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
    }
}