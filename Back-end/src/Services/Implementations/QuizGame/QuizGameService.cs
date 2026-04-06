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

    /// Verify the answer selected by the user for the current question. 
    /// <param name="currentUser">The user who is currently playing.</param>
    /// <param name="answer">The user's selected answer.</param>
    public void AnswerQuiz(CurrentUser currentUser, QuizGameResponse answer)
    {
        ValidateUser(currentUser);
        if (answer.answer.Trim().Equals(string.Empty))
        {
            throw new InvalidOperationException("Answer provided is empty");
        }
       
        quizGameConnector.AnswerQuiz(FetchUser(currentUser), answer.answer);
    }

    /// Get the current game statistics, including the number of correct, incorrect and skipped pairs.
    /// <param name="currentUser">The user who is currently playing.</param>
    /// Returns a QuizGameStats object containing the stats.
    public QuizGameStats GetGameStats(CurrentUser currentUser)
    {
        ValidateUser(currentUser);
        return quizGameConnector.GetGameStats(FetchUser(currentUser));
    }

    /// Get the next pair of sentences for the quiz. 
    /// <param name="currentUser">The user who is currently playing.</param>
    /// Returns the next QuizItem containing a sentence pair to quiz the user on.
    public QuizItem? GetNextQuiz(CurrentUser currentUser)
    {
        ValidateUser(currentUser);
        return quizGameConnector.GetNextQuiz(FetchUser(currentUser));
    }

    /// Initialize a job list for the game based on the provided filters. 
    /// <param name="currentUser">The user who is currently playing.
    public void InitializeSession(CurrentUser currentUser)
    {
        ValidateUser(currentUser);
        quizGameConnector.InitializeSession(FetchUser(currentUser));
    }

    /// Verify that the user is valid.  
    /// <param name="currentUser">The user to verify.</param>
    private void ValidateUser(CurrentUser currentUser)
    {
        if (currentUser.UserId < 0)
        {
            throw new InvalidOperationException("UserId provided is negative");
        }
    }

    /// Get a User based on a currentUser. 
    /// <param name="currentUser">The user details to fetch a User for.</param>
    /// Returns the matching User.
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