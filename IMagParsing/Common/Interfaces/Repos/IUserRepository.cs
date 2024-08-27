using IMagParsing.Core.Models;

namespace IMagParsing.Common.Interfaces.Repos;

public interface IUserRepository
{
    Task<User[]> GetSubscribers();
    Task<User?> GetUser(long userId);
    Task AddUserAsync(User user, CancellationToken cancellationToken = default);
    Task Update(User user, CancellationToken cancellationToken = default);
}