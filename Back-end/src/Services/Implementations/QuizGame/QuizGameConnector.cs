using Back_end.Persistence.Implementations;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

//Holds all users game sessions for the quiz game
//the game is indexed by there user id
public class QuizGameConnector : IQuizGameConnector
{
    private IQuizItemFetcherFactory quizItemFetcherFactory;
    private Dictionary<int, IQuizGame> gameServiceList = new Dictionary<int, IQuizGame>();
    public QuizGameConnector(IQuizItemFetcherFactory quizItemFetcherFactory)
    {
        this.quizItemFetcherFactory = quizItemFetcherFactory;
    }

    /// Initialize a job list for the game based on the provided filters. 
    /// <param name="users">The user who is currently playing.
    public void InitializeSession(User user)
    {
        gameServiceList[user.UserId] = new QuizGame(quizItemFetcherFactory.BuildFetcher());
    }

    /// Verify the answer selected by the user for the current question. 
    /// <param name="user">The user who is currently playing.</param>
    /// <param name="answer">The user's selected answer.</param>
    public void AnswerQuiz(User user, string answer)
    {
        CheckUserSession(user);
        gameServiceList[user.UserId].AnswerQuiz(answer);
    }

    /// Get the current game statistics, including the number of correct, incorrect and skipped pairs.
    /// <param name="user">The user who is currently playing.</param>
    /// Returns a QuizGameStats object containing the stats.
    public QuizGameStats GetGameStats(User user)
    {
        CheckUserSession(user);
        return gameServiceList[user.UserId].GetQuizGameStats();
    }

    /// Get the next pair of sentences for the quiz. 
    /// <param name="user">The user who is currently playing.</param>
    /// Returns the next QuizItem containing a sentence pair to quiz the user on.
    public QuizItem? GetNextQuiz(User user)
    {
        CheckUserSession(user);
        return gameServiceList[user.UserId].GetNextQuiz();
    }

    /// Verify a user is currently playing an initialized game. 
    /// <param name="users">The user who is currently playing.
    private void CheckUserSession(User user)
    {
        if (!gameServiceList.ContainsKey(user.UserId))
        {
            throw new InvalidOperationException($"No Session Found for {user.UserId} for Quiz Game");
        }
    }
}