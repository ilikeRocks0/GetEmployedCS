using Back_end.Endpoints.Models.NewUser;
using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Services.Interfaces;

public class UserService(IUserPersistence userPersistence) : IUserService
{
    public int CreateUser(NewUser newUser)
    {
        User savedUser = ExtractUserFromInput(newUser);
        return userPersistence.CreateUser(savedUser);
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