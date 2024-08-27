using IMagParsing.Common.Interfaces.Repos;
using IMagParsing.Core.Models;
using IMagParsing.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IMagParsing.Repos;

public class UserRepository(ProductsContext context) : IUserRepository
{
    public async Task AddUserAsync(User user, CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(User user, CancellationToken cancellationToken)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<User[]> GetSubscribers()
    {
        return await context.Users
            .Where(u => u.IsSubscribe == true)
            .ToArrayAsync();
    }

    public async Task<User?> GetUser(long userId)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
    }
}