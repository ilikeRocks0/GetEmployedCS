using Back_end.Persistence.Objects;

namespace Back_end.Persistence.Interfaces;

public interface IUserPersistence
{
    public User? GetUser(int userId);
    public User? GetUserByUsername(string username);
    public int CreateUser(User newUser);
    public int SaveJob(int userId, int jobId);
    public bool UnsaveJob(int userId, int jobId);
    public bool IsJobInLikes(int userId, int jobId);
    public User? GetUserByCredentials(string email, string password);
}
