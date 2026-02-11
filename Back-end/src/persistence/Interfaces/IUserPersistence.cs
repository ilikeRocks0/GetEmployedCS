using Back_end.Persistence.Objects;

namespace Back_end.Persistence.Interfaces;

public interface IUserPersistence
{
  public User? GetUser(int userId);
  public int CreateUser(User newUser);
}
