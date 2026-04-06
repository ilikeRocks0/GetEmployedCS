using Back_end.Endpoints.Models;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Implementations.Finders;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class UserGameService(IUserPersistence userPersistence) : IUserGameService
{
    private readonly UserGameConnector userGameConnector = new(userPersistence);
    private readonly UserFinder userFinder = new(userPersistence);

    /// Initialize a user list for the game.
    /// <param name="currentUser">The user accessing the game.
    /// Returns a random user profile to start the game.
    public Profile? InitializeUserGame(CurrentUser currentUser)
    {
        if (currentUser is null)
        {
            throw new InvalidOperationException("No UserId provided");
        }

        User? user = userFinder.GetUser(currentUser.UserId);
        if (user is null)
        {
            throw new InvalidOperationException("UserId doesn't match an existing user");
        }

        User? gameUser = userGameConnector.InitializeUserGame(user);
        return MapProfile(gameUser);
    }
    
    /// Reject the current user. The game statistics are updated to reflect the rejection.
    /// <param name="currentUser">The user accessing the game.
    /// <param name="username">The username of the user being rejected.
    /// Returns the next user profile in the game.
    public Profile? RejectUser(CurrentUser currentUser, string username)
    {
        if (currentUser is null || string.IsNullOrWhiteSpace(username))
        {
            throw new InvalidOperationException("Invalid format for current user id and username provided");
        }

        User? actorUser = userFinder.GetUser(currentUser.UserId);
        User? targetUser = userFinder.GetUserByUsername(username);
        if (actorUser is null || targetUser is null)
        {
            throw new InvalidOperationException("UserId or username doesn't match an existing user");
        }

        User? nextUser = userGameConnector.RejectUser(actorUser, targetUser);
        return MapProfile(nextUser);
    }

    /// Accept the current user. The game statistics are updated to reflect the acceptance.
    /// <param name="currentUser">The user accessing the game.
    /// <param name="username">The username of the user being accepted.
    /// Returns the next user profile in the game.
    public Profile? AcceptUser(CurrentUser currentUser, string username)
    {
        if (currentUser is null || string.IsNullOrWhiteSpace(username))
        {
            throw new InvalidOperationException("Invalid format for current user id and username provided");
        }

        User? actorUser = userFinder.GetUser(currentUser.UserId);
        User? targetUser = userFinder.GetUserByUsername(username);
        if (actorUser is null || targetUser is null)
        {
            throw new InvalidOperationException("UserId or username doesn't match an existing user");
        }

        User? nextUser = userGameConnector.AcceptUser(actorUser, targetUser);
        return MapProfile(nextUser);
    }

    /// Get the current game statistics, including the number of accepted and rejected users.
    /// <param name="currentUser">The user accessing the game.
    /// Returns a tuple containing the number of accepted and rejected users.
    public (int accepted, int rejected) GetGameStats(CurrentUser currentUser)
    {
        if (currentUser is null)
        {
            throw new InvalidOperationException("No UserId provided");
        }

        User? user = userFinder.GetUser(currentUser.UserId);
        if (user is null)
        {
            throw new InvalidOperationException("UserId doesn't match an existing user");
        }

        return userGameConnector.GetGameStats(user);
    }

    private static Profile? MapProfile(User? user)
    {
        if (user is null)
        {
            return null;
        }

        return new Profile(user.UserId, user.Username, user.Email, user.FirstName, user.LastName, user.About, user.Experiences, user.IsEmployer, user.EmployerName);
    }
}
