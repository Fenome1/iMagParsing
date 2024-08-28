using IMagParsing.ViewModels;

namespace IMagParsing.Services.Interfaces;

public interface IUserStateService
{
    Task<UserState?> Get(long userId);
    Task SaveUserState(UserState state);
    Task DeleteUserStateAsync(long userId);
    Task ClearAllStatesAsync();
}