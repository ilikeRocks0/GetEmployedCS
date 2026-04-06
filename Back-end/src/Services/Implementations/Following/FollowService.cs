using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Interfaces;
using Back_end.Util;

namespace Back_end.Services.Implementations;

public class FollowService (IUserPersistence userPersistence) : IFollowService
{
    /// Follows a user.
    /// <param name="userId">An id for the user doing the following.
    /// <param name="usernameToFollow">The username of the user account to be followed. 
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

    ///Unfollows a user.
    /// <param name="userId">An id for the user doing the unfollowing.
    /// <param name="usernameToFollow">The username of the user account to be unfollowed.
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

    ///checks if a user follows another user.
    /// <param name="userId">An id for the user to check the following list of .
    /// <param name="username">The username of the user account to check for.
    /// Returns true if the userId user is following the username user, or false if they don't.
    public bool IsFollowing(int userId, string username)
    {
        User? user = userPersistence.GetUserByUsername(username);
        if (user == null) return false;
        return userPersistence.IsUserInFollows(userId, user.UserId);
    }
}