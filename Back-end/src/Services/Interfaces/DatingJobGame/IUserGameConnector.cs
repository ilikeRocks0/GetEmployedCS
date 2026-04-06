using Back_end.Objects;

namespace Back_end.Services.Interfaces;

public interface IUserGameConnector
{
    /// Initialize a user list for the game.
    /// <param name="currentUser">The user accessing the game.
    /// Returns a random user to start the game.
    User? InitializeUserGame(User currentUser);

    /// Reject the current user. The game statistics are updated to reflect the rejection.
    /// <param name="currentUser">The user accessing the game.
    /// <param name="user">The the user being accepted.
    /// Returns the next user in the game.
    User? RejectUser(User currentUser, User user);

    /// Accept the current user. The game statistics are updated to reflect the acceptance.
    /// <param name="currentUser">The user accessing the game.
    /// <param name="user">The the user being accepted.
    /// Returns the next user in the game.
    User? AcceptUser(User currentUser, User user);

    /// Get the current game statistics, including the number of accepted and rejected users.
    /// <param name="currentUser">The user accessing the game.
    /// Returns a tuple containing the number of accepted and rejected users.
    (int accepted, int rejected) GetGameStats(User currentUser);
}
