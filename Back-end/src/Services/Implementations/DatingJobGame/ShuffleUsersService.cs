using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class ShuffleUsersService : IUserIndexManager
{
    private readonly IUserIndexManager userIndexManager;

    public ShuffleUsersService(IUserPersistence userPersistence)
    {
        userIndexManager = new UserIndexManager(userPersistence);
    }

    /// Get a list of users. 
    /// Returns a list of all users.
    public List<User> GetUsers()
    {
        return ShuffleUsers(userIndexManager.GetUsers());
    }

    /// Updates what the current user is. 
    /// <param name="currentUserId">The id of the user to set as the current user.
    public void UpdateCurrentUser(int currentUserId)
    {
        userIndexManager.UpdateCurrentUser(currentUserId);
    }
    
    /// Get a shuffled version of a list of users. 
    /// <param name="users">The list of users to shuffle.
    /// Returns a shuffled version of the users parameter.
    private static List<User> ShuffleUsers(List<User> users)
    {
        Random rand = new();
        int n = users.Count;
        for (int i = 0; i < n; i++)
        {
            int j = rand.Next(i, n);
            (users[j], users[i]) = (users[i], users[j]);
        }

        return users;
    }
}
