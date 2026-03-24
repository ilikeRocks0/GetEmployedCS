using Back_end.Endpoints.Models;

namespace Back_end.Services.Interfaces;

public interface IUserService
{
    int CreateUser(NewUser newUser);
    Profile? GetProfile(int userId);
    Profile? GetProfileByUsername(string username);
    int SaveJob(IReadOnlyDictionary<string, string>? filters = null);
    bool UnsaveJob(IReadOnlyDictionary<string, string>? filters = null);
    int Login(LoginRequest loginRequest);
    bool CheckUserEmployer(int userId);
}