using Back_end.Endpoints.Models;
using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
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
}