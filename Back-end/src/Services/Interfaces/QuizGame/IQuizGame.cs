using Back_end.Objects;

namespace Back_end.Services.Interfaces;
public interface IQuizGame
{
    /// Get the next pair of sentences for the quiz. 
    /// Returns the next QuizItem containing a sentence pair to quiz the user on.
    public QuizItem? GetNextQuiz();

    /// Verify the answer selected by the user for the current question. 
    /// <param name="answer">The user's selected answer.</param>
    public void AnswerQuiz(string answer);

    /// Get the current game statistics, including the number of correct, incorrect and skipped pairs.
    /// Returns a QuizGameStats object containing the stats.
    public QuizGameStats GetQuizGameStats();
}