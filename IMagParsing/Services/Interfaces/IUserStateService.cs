using IMagParsing.ViewModels;

namespace IMagParsing.Services.Interfaces;

public interface IUserStateService
{
    UserState? Get(long userId);
    void SaveUserStateAsync(long userId, UserState state);
    void DeleteUserStateAsync(long userId);
    void ClearAllStatesAsync();
}