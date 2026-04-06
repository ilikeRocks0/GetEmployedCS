using Back_end.Persistence.Interfaces;
using Back_end.Objects;

namespace Back_end.Services.Implementations.Finders;

public class UserFinder (IUserPersistence userPersistence)
{
    /// Retrieves a user based on a userId, if there is one.
    /// <param name="userId">An id for a user.
    /// Returns the user corresponding to the userId. 
    public User? GetUser(int userId)
    {
        return userPersistence.GetUser(userId);
    }

    /// Retrieves a user based on a username, if there is one.
    /// <param name="username">An username for a user.
    /// Returns the user corresponding to the username. 
    public User? GetUserByUsername(string username)
    {
        return userPersistence.GetUserByUsername(username);
    }
}