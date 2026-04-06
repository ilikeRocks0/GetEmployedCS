using Back_end.Endpoints.Models;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class QuizGameSingleton : IQuizGameService
{
    private IQuizGameService server;
    
    public QuizGameSingleton(IServiceScopeFactory scopeFactory)
    {
        using var scope = scopeFactory.CreateScope();
        IUserPersistence userPersistence = scope.ServiceProvider.GetRequiredService<IUserPersistence>();
        IQuizItemsPersistence quizItemsPersistence = scope.ServiceProvider.GetRequiredService<IQuizItemsPersistence>();

        server = new QuizGameService(userPersistence, quizItemsPersistence);
    }

    /// Verify the answer selected by the user for the current question. 
    /// <param name="currentUser">The user who is currently playing.</param>
    /// <param name="answer">The user's selected answer.</param>
    public void AnswerQuiz(CurrentUser currentUser, QuizGameResponse answer)
    {
        if (server == null) throw new InvalidOperationException("Quiz Game Server not initialized.");
        server.AnswerQuiz(currentUser, answer);
    }

    /// Get the current game statistics, including the number of correct, incorrect and skipped pairs.
    /// <param name="currentUser">The user who is currently playing.</param>
    /// Returns a QuizGameStats object containing the stats.
    public QuizGameStats GetGameStats(CurrentUser currentUser)
    {
        if (server == null) throw new InvalidOperationException("Quiz Game Server not initialized.");
        return server.GetGameStats(currentUser);
    }

    /// Get the next pair of sentences for the quiz. 
    /// <param name="currentUser">The user who is currently playing.</param>
    /// Returns the next QuizItem containing a sentence pair to quiz the user on.
    public QuizItem? GetNextQuiz(CurrentUser currentUser)
    {
        if (server == null) throw new InvalidOperationException("Quiz Game Server not initialized.");
        return server.GetNextQuiz(currentUser);
    }

    /// Initialize a job list for the game based on the provided filters. 
    /// <param name="currentUser">The user who is currently playing.
    public void InitializeSession(CurrentUser currentUser)
    {
        if (server == null) throw new InvalidOperationException("Quiz Game Server not initialized.");
        server.InitializeSession(currentUser);
    }
}