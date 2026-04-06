using Back_end.Objects;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class UserSwipeGameService(IUserIndexManager userIndexManager) : IUserSwipeGameService
{
    private int userAccepted = 0;
    private int userRejected = 0;
    private bool isGameInitialized = false;
    private bool hasActiveUser = false;
    private List<User> allUsers = [];

    /// Retrieves the job at the current index from the list of all users, then increments the index and wraps it around to the start of the list when it reaches the end.
    /// <param name="initialize">A flag for whether the game has been initialized or not. If not provided, defaults to false.
    /// <param name="isAccepted">A flag for whether the last user has been accepted or not. If not provided, defaults to null.
    /// Returns the next user to display. 
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

    /// Initialize a user list for the game.
    /// Returns a random user to start the game.
    public User? InitializeUserGame()
    {
        userAccepted = 0;
        userRejected = 0;
        isGameInitialized = true;
        hasActiveUser = false;
        allUsers.Clear();

        return GetGameUser(initialize: true);
    }

    /// Reject the current user. The game statistics are updated to reflect the rejection.
    /// Returns the next user in the game.
    public User? RejectUser()
    {
        if (!isGameInitialized)
        {
            throw new InvalidOperationException("Game not initialized. Please call InitializeUserGame() before rejecting users.");
        }

        return GetGameUser(isAccepted: false);
    }

    /// Accept the current user. The game statistics are updated to reflect the acceptance.
    /// Returns the next user in the game.

    public User? AcceptUser()
    {
        if (!isGameInitialized)
        {
            throw new InvalidOperationException("Game not initialized. Please call InitializeUserGame() before accepting users.");
        }

        return GetGameUser(isAccepted: true);
    }

    /// Get the current game statistics, including the number of accepted and rejected users.
    /// Returns a tuple containing the number of accepted and rejected users.
    public (int accepted, int rejected) GetGameStats()
    {
        if (!isGameInitialized)
        {
            throw new InvalidOperationException("Game not initialized. Please call InitializeUserGame() before getting game stats.");
        }

        return (userAccepted, userRejected);
    }
}
