using Back_end.Persistence.Objects;

namespace Back_end.Services.Interfaces;

public interface IUserSwipeGameService
{
    User? InitializeUserGame();
    User? RejectUser();
    User? AcceptUser();
    (int accepted, int rejected) GetGameStats();
}
