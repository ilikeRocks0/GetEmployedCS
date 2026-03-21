using Back_end.Endpoints.Models;
namespace Back_end.Services.Interfaces;
public interface IQuizGameService
{
    public void InitializeSession(CurrentUser currentUser);

    public QuizItem? GetNextQuiz(CurrentUser currentUser);

    public void AnswerQuiz(CurrentUser currentUser, QuizGameResponse answer);
    
    public QuizGameStats GetGameStats(CurrentUser currentUser);
}