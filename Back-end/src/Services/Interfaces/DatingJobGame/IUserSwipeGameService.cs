using Back_end.Objects;

namespace Back_end.Services.Interfaces;

public interface IUserSwipeGameService
{
    /// Initialize a user list for the game.
    /// Returns a random user to start the game.
    User? InitializeUserGame();

    /// Reject the current user. The game statistics are updated to reflect the rejection.
    /// Returns the next user in the game.
    User? RejectUser();

    /// Accept the current user. The game statistics are updated to reflect the acceptance.
    /// Returns the next user in the game.
    User? AcceptUser();

    /// Get the current game statistics, including the number of accepted and rejected users.
    /// Returns a tuple containing the number of accepted and rejected users.
    (int accepted, int rejected) GetGameStats();
}
