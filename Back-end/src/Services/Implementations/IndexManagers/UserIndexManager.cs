using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Interfaces;
using Back_end.Util;

namespace Back_end.Services.Implementations;

public class UserIndexManager(IUserPersistence userPersistence) : IUserIndexManager
{
    private int currentUserId;
    private int currentPage = 1;

    /// Get a list of users. 
    /// Returns a list of all users.
    public List<User> GetUsers()
    {
        List<User> users = userPersistence.GetUsersForGame(
            currentUserId,
            (currentPage - 1) * AppConfig.ITEMS_PER_PAGE,
            AppConfig.ITEMS_PER_PAGE
        );

        currentPage += 1;

        return users;
    }

    /// Updates what the current user is. 
    /// <param name="currentUserId">The id of the user to set as the current user.
    public void UpdateCurrentUser(int currentUserId)
    {
        this.currentUserId = currentUserId;
    }
}
