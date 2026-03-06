using Back_end.Endpoints.Models;
using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Services.Implementations.Finders;
using Back_end.Services.Interfaces;
using Back_end.Util;

public class UserService(IUserPersistence userPersistence) : IUserService
{
    public int CreateUser(NewUser newUser)
    {
        User savedUser = ExtractUserFromInput(newUser);
        return userPersistence.CreateUser(savedUser);
    }

    public int SaveJob(IReadOnlyDictionary<string, string>? filters = null)
    {
        if (filters == null)
        {
            throw new InvalidOperationException("Invalid filter parameters");
        }
        var userId = int.TryParse(filters.GetValueOrDefault(AppConfig.FilterKeys.USERID), out var uId) ? uId : 0;
        var jobId = int.TryParse(filters.GetValueOrDefault(AppConfig.FilterKeys.USERID), out var jId) ? jId : 0;
        
        if (userPersistence.IsJobInLikes(userId, jobId))
        {
            throw new InvalidOperationException("This job has already been liked by this user");
        }
        return userPersistence.SaveJob(userId, jobId);
    }

    private static User ExtractUserFromInput(NewUser newUser)
    {
        User savedUser;
        if (newUser.IsEmployer)
        {
            savedUser = new User(
                -1, 
                newUser.Email, 
                newUser.Username,
                newUser.Password, 
                "", 
                newUser.EmployerName);
        }
        else
        {
            savedUser = new User(
                -1,
                newUser.Email,
                newUser.Username, 
                newUser.Password, 
                "",
                newUser.FirstName,
                newUser.LastName, 
                []);
        }
        
        return savedUser;

    }

    public Profile? GetProfile(int userId)
    {
        UserFinder userFinder = new UserFinder(userPersistence);
        User? user = userFinder.GetUser(userId);
        Profile? profile = null; 
        if (user != null)
        {
            profile = new Profile(user.UserId, user.Username, user.Email, user.FirstName, user.LastName, user.About, user.Experiences, user.IsEmployer, user.EmployerName);
        }
        return profile;
    }
}