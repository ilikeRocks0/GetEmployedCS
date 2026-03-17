using Back_end.Endpoints.Models;

namespace Back_end.Services.Interfaces;

public interface IUserService
{
    int CreateUser(NewUser newUser);
    Profile? GetProfile(int user);
    int SaveJob(IReadOnlyDictionary<string, string>? filters = null);
    bool UnsaveJob(IReadOnlyDictionary<string, string>? filters = null);
}