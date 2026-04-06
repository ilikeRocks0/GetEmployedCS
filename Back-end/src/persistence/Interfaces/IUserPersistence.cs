using Back_end.Objects;

namespace Back_end.Persistence.Interfaces;

public interface IUserPersistence
{
    //<summary>
    //Gets a user from a userId.
    //</summary>
    //<param name="jobId">The target user's userId.</param>
    //<returns>A matching User object if found, else returns null.</returns>
    public User? GetUser(int userId);

    //<summary>
    //Gets a list of users based on the params given.
    //</summary>
    //<param name="searchTerm">A keyword string to filter users by name, username, or email.</param>
    //<param name="employer">A flag for whether to find employers (true) or not (false).</param>
    //<param name="startIndex">The index of the record in the Users table to start returning entities.</param>
    //<param name="pageSize">The max amount of objects to return.</param>
    //<returns>A list of Users.</returns>
    public List<User> GetUsers(string searchTerm, bool employer, int startIndex, int pageSize);

    //<summary>
    //Gets a list of users for the user game (employer side of job game).
    //</summary>
    //<param name="currentUserId">A keyword string to filter users by name, username, or email.</param>
    //<param name="startIndex">The index of the record in the Users table to start returning entities.</param>
    //<param name="pageSize">The max amount of users to return.</param>
    //<returns>A list of Users.</returns>
    public List<User> GetUsersForGame(int currentUserId, int startIndex, int amount);

    //<summary>
    //Gets a user from a username.
    //</summary>
    //<param name="username">The target user's username.</param>
    //<returns>A matching User object if found, else returns null.</returns>
    public User? GetUserByUsername(string username);

    //<summary>
    //Creates a new user based on a User object.
    //</summary>
    //<param name="newUser">The user to be created.</param>
    //<returns>The ID of the newly created user in the database.</returns>
    public (int userId, string verifyToken) CreateUser(User newUser);

    //<summary>
    //Saves a job for the user.
    //</summary>
    //<param name="userId">The id of the user to save the job for.</param>
    //<param name="jobId">The id of the job to save.</param>
    //<returns>0 if successful, else returns -1.</returns>
    public int SaveJob(int userId, int jobId);

    //<summary>
    //Unsaves a saved job for the user.
    //</summary>
    //<param name="userId">The id of the user to unsave the job for.</param>
    //<param name="jobId">The id of the job to unsave.</param>
    //<returns>True if successful, else returns false.</returns>
    public bool UnsaveJob(int userId, int jobId);

    //<summary>
    //Checks if a job is in a user's liked (saved) jobs list.
    //</summary>
    //<param name="userId">The id of the user to check the likes list for.</param>
    //<param name="jobId">The id of the job to check for.</param>
    //<returns>True if successful, else returns false.</returns>
    public bool IsJobInLikes(int userId, int jobId);

    //<summary>
    //Gets a user if the provided credentials match theirs. 
    //</summary>
    //<param name="email">The email to find a user for.</param>
    //<param name="password">The password to find a user for.</param>
    //<returns>A matching user if successful, else returns null.</returns>
    public User? GetUserByCredentials(string email, string password);

    //<summary>
    //Checks is a user is an employer. 
    //</summary>
    //<param name="userId">The id of the user to check.</param>
    //<returns>True if the user is an employer, else returns false.</returns>
    public bool CheckUserEmployer(int userId);

    //<summary>
    //Causes a user to follow another user. 
    //</summary>
    //<param name="followerId">The id of the user following another user.</param>
    //<param name="followedId">The id of the user being followed.</param>
    public void FollowUser(int followerId, int followedId);

    //<summary>
    //Causes a user to unfollow another user. 
    //</summary>
    //<param name="followerId">The id of the user following another user.</param>
    //<param name="followedId">The id of the user being unfollowed.</param>
    public void UnfollowUser(int followerId, int followedId);

    //<summary>
    //Checks if a user is following another user.
    //</summary>
    //<param name="followerId">The id of the user following another user.</param>
    //<param name="followedId">The id of the user being followed.</param>
    //<returns>True if the followerId user follows the followedId user, else returns false.</returns>
    public bool IsUserInFollows(int followerId, int followedId);

    //<summary>
    //Gets a list of all the followers of a user.
    //</summary>
    //<param name="userId">The id of the user with the follows.</param>
    //<returns>A list of users.</returns>
    public List<User> GetAllFollowers(int userId);

    //<summary>
    //Gets a list of all the users a user is following.
    //</summary>
    //<param name="userId">The id of the user who follows other users.</param>
    //<returns>A list of users.</returns>
    public List<User> GetAllFollowing(int userId);

    //<summary>
    //Updates a user's attributes.
    //</summary>
    //<param name="updatedUser">A User object with the updated attributes.</param>
    public void UpdateUser(User updatedUser);

    //<summary>
    //Adds a new experience for a user.
    //</summary>
    //<param name="userId">The id of the user to add the experience to.</param>
    //<param name="experience">The experience to add.</param>
    //<returns>The ID newly added experience.</returns>
    public int CreateExperience(int userId, Experience experience);

    //<summary>
    //Gets a list of experiences belonging to a user.
    //</summary>
    //<param name="userId">The id of the user to to get the experienecs of.</param>
    //<returns>A list of Experiences.</returns>
    public List<Experience> GetExperiences(int userId);

    //<summary>
    //Updates a user's experience.
    //</summary>
    //<param name="userId">The id of the user who has the experience.</param>
    //<param name="oldExperience">The experience being updated, used to find the matching experience for the user.</param>
    //<param name="newExperience">An experience with the attributes to use to update.</param>
    public void UpdateExperience(int userId, Experience oldExperience, Experience newExperience);

    //<summary>
    //Deletes a user's experience.
    //</summary>
    //<param name="userId">The id of the user who has the experience.</param>
    //<param name="experience">The experience to delete, used to find the matching experience for the user.</param>
    public void DeleteExperience(int userId, Experience experience);

    //<summary>
    //Checks if the user owns the specified experience.
    //</summary>
    //<param name="userId">The id of the user to check the experience for.</param>
    //<param name="experienceId">The id of the experience to check if it belongs to the user.</param>
    //<returns>True if the user owns the experience, else return false.</returns>
    public bool IsExperienceOwner(int userId, int experienceId);

    //<summary>
    //Marks a user as verified.
    //</summary>
    //<param name="token">The token to find a matching user for and verify them if found.</param>
    public void VerifyUser(string token);

    //<summary>
    //Add a new comment under a user profile.
    //</summary>
    //<param name="comment">The comment to add to the profile.</param>
    //<returns>The newly created UserComment.</returns>
    public UserComment CreateUserComment(UserComment comment);

    //<summary>
    //Gets a list of comments on a user profile.
    //</summary>
    //<param name="userId">The id of the user to get profile comments for.</param>
    //<returns>A list of UserComments.</returns>
    public List<UserComment> GetProfileComments(int userId);
}
