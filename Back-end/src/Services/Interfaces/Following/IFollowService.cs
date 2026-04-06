namespace Back_end.Services.Interfaces;
public interface IFollowService
{
    /// Follows a user.
    /// <param name="userId">An id for the user doing the following.
    /// <param name="usernameToFollow">The username of the user account to be followed. 
    public void FollowUser(int userId, string usernameToFollow);

    ///Unfollows a user.
    /// <param name="userId">An id for the user doing the unfollowing.
    /// <param name="usernameToFollow">The username of the user account to be unfollowed. 
    public void UnfollowUser(int userId, string usernameToUnfollow);
    
    ///checks if a user follows another user.
    /// <param name="userId">An id for the user to check the following list of .
    /// <param name="username">The username of the user account to check for.
    /// Returns true if the userId user is following the username user, or false if they don't.
    public bool IsFollowing(int userId, string username);
}