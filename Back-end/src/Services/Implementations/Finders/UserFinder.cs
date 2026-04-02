using Back_end.Persistence.Interfaces;
using Back_end.Objects;

namespace Back_end.Services.Implementations.Finders;

public class UserFinder (IUserPersistence userPersistence)
{
    public User? GetUser(int userId)
    {
        return userPersistence.GetUser(userId);
    }

    public User? GetUserByUsername(string username)
    {
        return userPersistence.GetUserByUsername(username);
    }
}