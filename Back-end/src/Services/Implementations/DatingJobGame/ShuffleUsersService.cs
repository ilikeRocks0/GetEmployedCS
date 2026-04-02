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

    public List<User> GetUsers()
    {
        return ShuffleUsers(userIndexManager.GetUsers());
    }

    public void UpdateCurrentUser(int currentUserId)
    {
        userIndexManager.UpdateCurrentUser(currentUserId);
    }
    
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
