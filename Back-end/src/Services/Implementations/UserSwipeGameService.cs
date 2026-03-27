using Back_end.Persistence.Objects;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class UserSwipeGameService(IUserIndexManager userIndexManager) : IUserSwipeGameService
{
    private int userAccepted = 0;
    private int userRejected = 0;
    private bool isGameInitialized = false;
    private bool hasActiveUser = false;
    private List<User> allUsers = [];

    private User? GetGameUser(bool initialize = false, bool? isAccepted = null)
    {
        if (!initialize && isAccepted.HasValue && hasActiveUser)
        {
            if (isAccepted.Value)
            {
                userAccepted++;
            }
            else
            {
                userRejected++;
            }

            hasActiveUser = false;
        }

        if (allUsers.Count == 0)
        {
            allUsers = userIndexManager.GetUsers();
        }

        if (allUsers.Count == 0)
        {
            return null;
        }

        User user = allUsers[0];
        allUsers.RemoveAt(0);
        hasActiveUser = true;

        return user;
    }

    public User? InitializeUserGame()
    {
        userAccepted = 0;
        userRejected = 0;
        isGameInitialized = true;
        hasActiveUser = false;
        allUsers.Clear();

        return GetGameUser(initialize: true);
    }

    public User? RejectUser()
    {
        if (!isGameInitialized)
        {
            throw new InvalidOperationException("Game not initialized. Please call InitializeUserGame() before rejecting users.");
        }

        return GetGameUser(isAccepted: false);
    }

    public User? AcceptUser()
    {
        if (!isGameInitialized)
        {
            throw new InvalidOperationException("Game not initialized. Please call InitializeUserGame() before accepting users.");
        }

        return GetGameUser(isAccepted: true);
    }

    public (int accepted, int rejected) GetGameStats()
    {
        if (!isGameInitialized)
        {
            throw new InvalidOperationException("Game not initialized. Please call InitializeUserGame() before getting game stats.");
        }

        return (userAccepted, userRejected);
    }
}
