using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;

namespace Back_end.Persistence.Implementations;

public class UserPersistence : IUserPersistence
{
  private IConfiguration config;

  public UserPersistence(IConfiguration config)
  {
    this.config = config;
  }

  public User GetUser(int userId)
  {
    User? user = null;

    using (AppDbContext context = new(this.config))
    {
      UserEntity userEntity = context.Users
        .Where(e => e.user_id == userId)
        .Single();
      
      user = new User(userEntity.username, userEntity.password);
    }

    return user;
  }

  public User GetUser(string username, string password)
  {
    User? user = null;

    using (AppDbContext context = new(this.config))
    {
      UserEntity userEntity = context.Users
        .Where(e => e.username.Equals(username))
        .Where(e => e.password.Equals(password))
        .Single();
      
      user = new User(userEntity.username, userEntity.password);
    }

    return user;
  }

  public void CreateUser(User newUser)
  {
    using (AppDbContext context = new(this.config))
    {
      UserEntity newUserEntity = new()
      {
        username = newUser.Username,
        password = newUser.Password
      };

      context.Users.Add(newUserEntity);

      context.SaveChanges();
    }
  }
}