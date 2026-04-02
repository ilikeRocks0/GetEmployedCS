using Back_end.Endpoints.Models;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Implementations.Finders;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class QuizGameService : IQuizGameService
{
    private IQuizGameConnector quizGameConnector;
    private UserFinder userFinder;
    public QuizGameService(IUserPersistence userPersistence, IQuizItemsPersistence quizItemsPersistence)
    {
        IQuizItemFetcherFactory quizItemFactory = new RandomQuizItemFetcherFactory(quizItemsPersistence); 
        userFinder = new UserFinder(userPersistence);
        quizGameConnector = new QuizGameConnector(quizItemFactory);
    }
    public void AnswerQuiz(CurrentUser currentUser, QuizGameResponse answer)
    {
        ValidateUser(currentUser);
        if (answer.answer.Trim().Equals(string.Empty))
        {
            throw new InvalidOperationException("Answer provided is empty");
        }
       
        quizGameConnector.AnswerQuiz(FetchUser(currentUser), answer.answer);
    }

    public QuizGameStats GetGameStats(CurrentUser currentUser)
    {
        ValidateUser(currentUser);
        return quizGameConnector.GetGameStats(FetchUser(currentUser));
    }

    public QuizItem? GetNextQuiz(CurrentUser currentUser)
    {
        ValidateUser(currentUser);
        return quizGameConnector.GetNextQuiz(FetchUser(currentUser));
    }

    public void InitializeSession(CurrentUser currentUser)
    {
        ValidateUser(currentUser);
        quizGameConnector.InitializeSession(FetchUser(currentUser));
    }

    private void ValidateUser(CurrentUser currentUser)
    {
        if (currentUser.UserId < 0)
        {
            throw new InvalidOperationException("UserId provided is negative");
        }
    }

    private User FetchUser(CurrentUser currentUser)
    {
        User? user = userFinder.GetUser(currentUser.UserId);
        if (user is null)
        {
            throw new InvalidOperationException("UserId doesn't match an existing user");
        }
        return user;
    }
}