using Back_end.Endpoints.Models;

namespace Back_end.Services.Interfaces;

public interface IUserGameService
{
    /// Initialize a user list for the game.
    /// <param name="currentUser">The user accessing the game.
    /// Returns a random user profile to start the game.
    Profile? InitializeUserGame(CurrentUser currentUser);

    /// Reject the current user. The game statistics are updated to reflect the rejection.
    /// <param name="currentUser">The user accessing the game.
    /// <param name="username">The username of the user being rejected.
    /// Returns the next user profile in the game.
    Profile? RejectUser(CurrentUser currentUser, string username);

    /// Accept the current user. The game statistics are updated to reflect the acceptance.
    /// <param name="currentUser">The user accessing the game.
    /// <param name="username">The username of the user being accepted.
    /// Returns the next user profile in the game.
    Profile? AcceptUser(CurrentUser currentUser, string username);

    /// Get the current game statistics, including the number of accepted and rejected users.
    /// <param name="currentUser">The user accessing the game.
    /// Returns a tuple containing the number of accepted and rejected users.
    (int accepted, int rejected) GetGameStats(CurrentUser currentUser);
}
