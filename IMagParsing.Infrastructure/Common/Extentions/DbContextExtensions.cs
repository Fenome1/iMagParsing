using Microsoft.EntityFrameworkCore;

namespace IMagParsing.Infrastructure.Common.Extentions;

public static class DbContextExtensions
{
    public static async Task WithTransactionAsync(this DbContext context, Func<Task> action,
        CancellationToken cancellationToken = default)
    {
        if (context.Database.CurrentTransaction != null)
        {
            await action();
        }
        else
        {
            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await action();
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }

    public static async Task<T> WithTransactionAsync<T>(this DbContext context, Func<Task<T>> action,
        CancellationToken cancellationToken = default)
    {
        if (context.Database.CurrentTransaction != null)
            return await action();

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await action();
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}