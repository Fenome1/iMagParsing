using IMagParsing.Core.Models;

namespace IMagParsing.Common.Interfaces;

public interface IUserService
{
    Task<User[]> GetAllUsers();
    Task<User[]> GetSubscribeUsers();
    Task AddUserAsync(User user);
    Task UpdateSubscribeStatusAsync(long userId, bool status);
}