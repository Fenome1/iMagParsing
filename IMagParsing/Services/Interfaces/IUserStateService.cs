using IMagParsing.ViewModels;

namespace IMagParsing.Services.Interfaces;

public interface IUserStateService
{
    Task<UserState?> GetAsync(long userId);
    Task SaveUserStateAsync(UserState state);
    Task DeleteUserStateAsync(long userId);
    Task ClearAllStatesAsync();
}