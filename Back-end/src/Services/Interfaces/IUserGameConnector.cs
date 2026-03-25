using Back_end.Persistence.Objects;

namespace Back_end.Services.Interfaces;

public interface IUserGameConnector
{
    User? InitializeUserGame(User currentUser);
    User? RejectUser(User currentUser, User user);
    User? AcceptUser(User currentUser, User user);
    (int accepted, int rejected) GetGameStats(User currentUser);
}
