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

    /// Initialize a user list for the game.
    /// <param name="currentUser">The user accessing the game.
    /// Returns a random user profile to start the game.
    public Profile? InitializeUserGame(CurrentUser currentUser)
    {
        if (userGameService == null) throw new InvalidOperationException("Service not initialized.");
        return userGameService.InitializeUserGame(currentUser);
    }

    /// Reject the current user. The game statistics are updated to reflect the rejection.
    /// <param name="currentUser">The user accessing the game.
    /// <param name="username">The username of the user being rejected.
    /// Returns the next user profile in the game.
    public Profile? RejectUser(CurrentUser currentUser, string username)
    {
        if (userGameService == null) throw new InvalidOperationException("Game not initialized.");
        return userGameService.RejectUser(currentUser, username);
    }

    /// Accept the current user. The game statistics are updated to reflect the acceptance.
    /// <param name="currentUser">The user accessing the game.
    /// <param name="username">The username of the user being accepted.
    /// Returns the next user profile in the game.
    public Profile? AcceptUser(CurrentUser currentUser, string username)
    {
        if (userGameService == null) throw new InvalidOperationException("Game not initialized.");
        return userGameService.AcceptUser(currentUser, username);
    }

    /// Get the current game statistics, including the number of accepted and rejected users.
    /// <param name="currentUser">The user accessing the game.
    /// Returns a tuple containing the number of accepted and rejected users.
    public (int accepted, int rejected) GetGameStats(CurrentUser currentUser)
    {
        if (userGameService == null) throw new InvalidOperationException("Game not initialized.");
        return userGameService.GetGameStats(currentUser);
    }
}
