using IMagParsing.Core.Models;

namespace IMagParsing.Repos.Interfaces;

public interface IUserRepository
{
    Task<User[]> GetSubscribers();
    Task<User?> GetUser(long userId);
    Task AddUserAsync(User user, CancellationToken cancellationToken = default);
    Task Update(User user, CancellationToken cancellationToken = default);
}