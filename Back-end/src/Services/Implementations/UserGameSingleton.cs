using Back_end.Endpoints.Models;
using Back_end.Persistence.Interfaces;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class UserGameSingleton : IUserGameService
{
    private IUserGameService? userGameService;

    public UserGameSingleton(IServiceScopeFactory scopeFactory)
    {
        using var scope = scopeFactory.CreateScope();
        var userPersistence = scope.ServiceProvider.GetRequiredService<IUserPersistence>();
        userGameService = new UserGameService(userPersistence);
    }

    public Profile? InitializeUserGame(CurrentUser currentUser)
    {
        if (userGameService == null) throw new InvalidOperationException("Service not initialized.");
        return userGameService.InitializeUserGame(currentUser);
    }

    public Profile? RejectUser(CurrentUser currentUser, string username)
    {
        if (userGameService == null) throw new InvalidOperationException("Game not initialized.");
        return userGameService.RejectUser(currentUser, username);
    }

    public Profile? AcceptUser(CurrentUser currentUser, string username)
    {
        if (userGameService == null) throw new InvalidOperationException("Game not initialized.");
        return userGameService.AcceptUser(currentUser, username);
    }

    public (int accepted, int rejected) GetGameStats(CurrentUser currentUser)
    {
        if (userGameService == null) throw new InvalidOperationException("Game not initialized.");
        return userGameService.GetGameStats(currentUser);
    }
}
