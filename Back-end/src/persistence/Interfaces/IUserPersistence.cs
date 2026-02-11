using Back_end.Persistence.Objects;

namespace Back_end.Persistence.Interfaces;

public interface IUserPersistence
{
  public User GetUser(int userId);
  public User GetUser(string username, string password);
  public void CreateUser(User newUser);
}
