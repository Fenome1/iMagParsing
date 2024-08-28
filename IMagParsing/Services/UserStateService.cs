using System.Collections.Concurrent;
using IMagParsing.Services.Interfaces;
using IMagParsing.ViewModels;

namespace IMagParsing.Services;

public class UserStateService : IUserStateService
{
    private readonly ConcurrentDictionary<long, UserState> _userStates = new();

    public UserState? Get(long userId)
    {
        _userStates.TryGetValue(userId, out var state);
        return state;
    }

    public void SaveUserState(UserState state)
    {
        _userStates[state.UserId] = state;
    }

    public void DeleteUserStateAsync(long userId)
    {
        _userStates.TryRemove(userId, out _);
    }

    public void ClearAllStatesAsync()
    {
        _userStates.Clear();
    }
}