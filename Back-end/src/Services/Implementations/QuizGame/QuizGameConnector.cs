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

    public void InitializeSession(User user)
    {
        gameServiceList[user.UserId] = new QuizGame(quizItemFetcherFactory.BuildFetcher());
    }

    public void AnswerQuiz(User user, string answer)
    {
        CheckUserSession(user);
        gameServiceList[user.UserId].AnswerQuiz(answer);
    }

    public QuizGameStats GetGameStats(User user)
    {
        CheckUserSession(user);
        return gameServiceList[user.UserId].GetQuizGameStats();
    }

    public QuizItem? GetNextQuiz(User user)
    {
        CheckUserSession(user);
        return gameServiceList[user.UserId].GetNextQuiz();
    }

    private void CheckUserSession(User user)
    {
        if (!gameServiceList.ContainsKey(user.UserId))
        {
            throw new InvalidOperationException($"No Session Found for {user.UserId} for Quiz Game");
        }
    }
}