using Back_end.Endpoints.Models.NewUser;

namespace Back_end.Services.Interfaces;

public interface IUserService
{
    int CreateUser(NewUser newUser);
}