using Back_end.Persistence.Objects;

namespace Back_end.Services.Interfaces;

public interface IUserIndexManager
{
    public List<User> GetUsers();
    public void UpdateCurrentUser(int currentUserId);
}