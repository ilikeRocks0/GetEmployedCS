using Back_end.Objects;
namespace Back_end.Services.Interfaces;
public interface IQuizGameConnector
{
    /// Initialize a job list for the game based on the provided filters. 
    /// <param name="users">The user who is currently playing.
    public void InitializeSession(User user);

    /// Get the next pair of sentences for the quiz. 
    /// <param name="user">The user who is currently playing.</param>
    /// Returns the next QuizItem containing a sentence pair to quiz the user on.
    public QuizItem? GetNextQuiz(User user);

    /// Verify the answer selected by the user for the current question. 
    /// <param name="user">The user who is currently playing.</param>
    /// <param name="answer">The user's selected answer.</param>
    public void AnswerQuiz(User user, string answer);
    
    /// Get the current game statistics, including the number of correct, incorrect and skipped pairs.
    /// <param name="user">The user who is currently playing.</param>
    /// Returns a QuizGameStats object containing the stats.
    public QuizGameStats GetGameStats(User user);
}