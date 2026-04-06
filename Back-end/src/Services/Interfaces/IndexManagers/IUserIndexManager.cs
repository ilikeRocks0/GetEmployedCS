using Back_end.Objects;

namespace Back_end.Services.Interfaces;

public interface IUserIndexManager
{
    /// Get a list of users. 
    /// Returns a list of all users.
    public List<User> GetUsers();

    /// Updates what the current user is. 
    /// <param name="currentUserId">The id of the user to set as the current user.
    public void UpdateCurrentUser(int currentUserId);
}