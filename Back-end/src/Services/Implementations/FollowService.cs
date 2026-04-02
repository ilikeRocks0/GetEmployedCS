using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Interfaces;
using Back_end.Util;

namespace Back_end.Services.Implementations;

public class FollowService (IUserPersistence userPersistence) : IFollowService
{
    public void FollowUser(int userId, string usernameToFollow)
    {

        User? userToFollow = userPersistence.GetUserByUsername(usernameToFollow);
        if (userToFollow == null)
        {
            throw new InvalidOperationException("User to follow not found");
        } 
        else
        {
            userPersistence.FollowUser(userId, userToFollow.UserId);
        }
    }

    public void UnfollowUser(int userId, string usernameToUnfollow)
    {
        User? userToUnfollow = userPersistence.GetUserByUsername(usernameToUnfollow);
        if (userToUnfollow == null)
        {
            throw new InvalidOperationException("User to unfollow not found");
        }
        else
        {
            userPersistence.UnfollowUser(userId, userToUnfollow.UserId);
        }
    }

    public bool IsFollowing(int userId, string username)
    {
        User? user = userPersistence.GetUserByUsername(username);
        if (user == null) return false;
        return userPersistence.IsUserInFollows(userId, user.UserId);
    }
}