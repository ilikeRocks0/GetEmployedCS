using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class UserGameConnector(IUserPersistence userPersistence) : IUserGameConnector
{
    private readonly Dictionary<int, IUserSwipeGameService> gameServiceList = [];

    /// Initialize a user list for the game.
    /// <param name="currentUser">The user accessing the game.
    /// Returns a random user to start the game.
    public User? InitializeUserGame(User currentUser)
    {
        var indexManager = new ShuffleUsersService(userPersistence);
        indexManager.UpdateCurrentUser(currentUser.UserId);
        gameServiceList[currentUser.UserId] = new UserSwipeGameService(indexManager);
        return gameServiceList[currentUser.UserId].InitializeUserGame();
    }
    
    /// Reject the current user. The game statistics are updated to reflect the rejection.
    /// <param name="currentUser">The user accessing the game.
    /// <param name="user">The the user being accepted.
    /// Returns the next user in the game.
    public User? RejectUser(User currentUser, User user)
    {
        if (!gameServiceList.ContainsKey(currentUser.UserId))
        {
            throw new InvalidOperationException("UserId doesn't match an existing user game session");
        }

        return gameServiceList[currentUser.UserId].RejectUser();
    }

    /// Accept the current user. The game statistics are updated to reflect the acceptance.
    /// <param name="currentUser">The user accessing the game.
    /// <param name="user">The the user being accepted.
    /// Returns the next user in the game.
    public User? AcceptUser(User currentUser, User user)
    {
        if (!gameServiceList.ContainsKey(currentUser.UserId))
        {
            throw new InvalidOperationException("UserId doesn't match an existing user game session");
        }

        //If we already saved it just dont save it again
        if (!userPersistence.IsUserInFollows(currentUser.UserId, (int)user.UserId!))
        {
            userPersistence.FollowUser(currentUser.UserId, (int)user.UserId);
        }

        return gameServiceList[currentUser.UserId].AcceptUser();
    }

    /// Get the current game statistics, including the number of accepted and rejected users.
    /// <param name="currentUser">The user accessing the game.
    /// Returns a tuple containing the number of accepted and rejected users.
    public (int accepted, int rejected) GetGameStats(User currentUser)
    {
        if (!gameServiceList.ContainsKey(currentUser.UserId))
        {
            throw new InvalidOperationException("UserId doesn't match an existing user game session");
        }

        return gameServiceList[currentUser.UserId].GetGameStats();
    }
}
