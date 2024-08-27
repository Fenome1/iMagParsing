using IMagParsing.Common.Interfaces;
using IMagParsing.Core.Models;
using IMagParsing.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IMagParsing.Services;

public class UserService(ProductsContext context) : IUserService
{
    public async Task<User[]> GetSubscribeUsers()
    {
        return await context.Users
            .Where(u => u.IsSubscribe == true)
            .ToArrayAsync();
    }

    public async Task AddUserAsync(User user)
    {
        if (await IsUserExists(user.UserId))
            return;

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task UpdateSubscribeStatusAsync(long userId, bool status)
    {
        var user = await GetUserAsync(userId);

        user.IsSubscribe = status;

        await context.SaveChangesAsync();
    }

    private async Task<User> GetUserAsync(long userId)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

        if (user is null)
            throw new Exception($"Пользователь с id:{userId} не найден");

        return user;
    }

    private async Task<bool> IsUserExists(long userId)
    {
        return await context.Users.AnyAsync(u => u.UserId == userId);
    }
}