using IMagParsing.Core.Models;

namespace IMagParsing.Repos.Interfaces;

public interface IUserRepository
{
    Task<User[]> GetSubscribersAsync();
    Task<User?> GetUserAsync(long userId);
    Task AddUserAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
}