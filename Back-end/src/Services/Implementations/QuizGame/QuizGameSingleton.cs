using Back_end.Endpoints.Models;
using Back_end.Persistence.Interfaces;
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
    public void AnswerQuiz(CurrentUser currentUser, QuizGameResponse answer)
    {
        if (server == null) throw new InvalidOperationException("Quiz Game Server not initialized.");
        server.AnswerQuiz(currentUser, answer);
    }

    public QuizGameStats GetGameStats(CurrentUser currentUser)
    {
        if (server == null) throw new InvalidOperationException("Quiz Game Server not initialized.");
        return server.GetGameStats(currentUser);
    }

    public QuizItem? GetNextQuiz(CurrentUser currentUser)
    {
        if (server == null) throw new InvalidOperationException("Quiz Game Server not initialized.");
        return server.GetNextQuiz(currentUser);
    }

    public void InitializeSession(CurrentUser currentUser)
    {
        if (server == null) throw new InvalidOperationException("Quiz Game Server not initialized.");
        server.InitializeSession(currentUser);
    }
}