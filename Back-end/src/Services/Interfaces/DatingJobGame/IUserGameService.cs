using Back_end.Endpoints.Models;

namespace Back_end.Services.Interfaces;

public interface IUserGameService
{
    Profile? InitializeUserGame(CurrentUser currentUser);
    Profile? RejectUser(CurrentUser currentUser, string username);
    Profile? AcceptUser(CurrentUser currentUser, string username);
    (int accepted, int rejected) GetGameStats(CurrentUser currentUser);
}
