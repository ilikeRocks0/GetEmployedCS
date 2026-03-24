using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class UserGameConnector(IUserPersistence userPersistence) : IUserGameConnector
{
    private readonly Dictionary<int, IUserSwipeGameService> gameServiceList = [];

    public User? InitializeUserGame(User currentUser)
    {
        var indexManager = new ShuffleUsersService(userPersistence);
        indexManager.UpdateCurrentUser(currentUser.UserId);
        gameServiceList[currentUser.UserId] = new UserSwipeGameService(indexManager);
        return gameServiceList[currentUser.UserId].InitializeUserGame();
    }
    
    public User? RejectUser(User currentUser, User user)
    {
        if (!gameServiceList.ContainsKey(currentUser.UserId))
        {
            throw new InvalidOperationException("UserId doesn't match an existing user game session");
        }

        return gameServiceList[currentUser.UserId].RejectUser();
    }

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

    public (int accepted, int rejected) GetGameStats(User currentUser)
    {
        if (!gameServiceList.ContainsKey(currentUser.UserId))
        {
            throw new InvalidOperationException("UserId doesn't match an existing user game session");
        }

        return gameServiceList[currentUser.UserId].GetGameStats();
    }
}
