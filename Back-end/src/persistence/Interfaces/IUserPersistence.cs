using Back_end.Objects;

namespace Back_end.Persistence.Interfaces;

public interface IUserPersistence
{
    public User? GetUser(int userId);
    public List<User> GetUsers(string searchTerm, bool employer, int startIndex, int pageSize);
    public List<User> GetUsersForGame(int currentUserId, int startIndex, int amount);
    public User? GetUserByUsername(string username);
    public int CreateUser(User newUser);
    public int SaveJob(int userId, int jobId);
    public bool UnsaveJob(int userId, int jobId);
    public bool IsJobInLikes(int userId, int jobId);
    public User? GetUserByCredentials(string email, string password);
    public bool CheckUserEmployer(int userId);
    public void FollowUser(int followerId, int followedId);
    public void UnfollowUser(int followerId, int followedId);
    public bool IsUserInFollows(int followerId, int followedId);
    public List<User> GetAllFollowers(int userId);
    public List<User> GetAllFollowing(int userId);
    public void UpdateUser(User updatedUser);
    public int CreateExperience(int userId, Experience experience);
    public List<Experience> GetExperiences(int userId);
    public void UpdateExperience(int userId, Experience oldExperience, Experience newExperience);
    public void DeleteExperience(int userId, Experience experience);
    public bool IsExperienceOwner(int userId, int experienceId);
    public void VerifyUser(string token);
    public UserComment CreateUserComment(UserComment comment);
    public List<UserComment> GetProfileComments(int userId);
}
