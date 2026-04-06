using Back_end.Endpoints.Models;
using Back_end.Objects;
namespace Back_end.Services.Interfaces;
public interface IQuizGameService
{
    /// Initialize a job list for the game based on the provided filters. 
    /// <param name="currentUser">The user who is currently playing.
    public void InitializeSession(CurrentUser currentUser);

    /// Get the next pair of sentences for the quiz. 
    /// <param name="currentUser">The user who is currently playing.</param>
    /// Returns the next QuizItem containing a sentence pair to quiz the user on.
    public QuizItem? GetNextQuiz(CurrentUser currentUser);

    /// Verify the answer selected by the user for the current question. 
    /// <param name="currentUser">The user who is currently playing.</param>
    /// <param name="answer">The user's selected answer.</param>
    public void AnswerQuiz(CurrentUser currentUser, QuizGameResponse answer);
    
    /// Get the current game statistics, including the number of correct, incorrect and skipped pairs.
    /// <param name="currentUser">The user who is currently playing.</param>
    /// Returns a QuizGameStats object containing the stats.
    public QuizGameStats GetGameStats(CurrentUser currentUser);
}