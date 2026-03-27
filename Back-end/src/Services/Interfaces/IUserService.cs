using Back_end.Endpoints.Models;
using Back_end.Persistence.Objects;

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
    Profile UpdateUser(UpdateUserRequest request, int userId);
    int AddExperience(int userId, Experience experience);
    void EditExperience(int userId, Experience oldExperience, Experience newExperience);
    void DeleteExperience(int userId, Experience experience);
}