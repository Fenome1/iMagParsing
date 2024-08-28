using System.Collections.Concurrent;
using IMagParsing.Services.Interfaces;
using IMagParsing.ViewModels;

namespace IMagParsing.Services;

public class UserStateService : IUserStateService
{
    private readonly ConcurrentDictionary<long, UserState> _userStates = new();

    public Task<UserState?> Get(long userId)
    {
        _userStates.TryGetValue(userId, out var state);
        return Task.FromResult(state);
    }

    public Task SaveUserState(UserState state)
    {
        _userStates[state.UserId] = state;
        return Task.CompletedTask;
    }

    public Task DeleteUserStateAsync(long userId)
    {
        _userStates.TryRemove(userId, out _);
        return Task.CompletedTask;
    }

    public Task ClearAllStatesAsync()
    {
        _userStates.Clear();
        return Task.CompletedTask;
    }
}