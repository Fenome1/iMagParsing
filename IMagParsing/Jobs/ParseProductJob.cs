using System;
using System.Threading.Tasks;
using IMagParsing.Common.Config;
using IMagParsing.Common.Interfaces;
using IMagParsing.Infrastructure;
using Quartz;

namespace IMagParsing.Jobs;

public class ParseProductJob(IProductParser parser, ProductsContext dbContext) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        foreach (var url in UrlData.ParsingUrls)
            try
            {
                var products = await parser.ParseImagProducts(url);

                await dbContext.ProductParsings.AddRangeAsync(products);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
    }
}