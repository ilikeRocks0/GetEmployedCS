namespace Back_end.Services.Interfaces;
public interface IFollowService
{
    //Follow a user
    public void FollowUser(int userId, string usernameToFollow);
    //Unfollow a user
    public void UnfollowUser(int userId, string usernameToUnfollow);
    //Check if a user is following another user
    public bool IsFollowing(int userId, string username);
}