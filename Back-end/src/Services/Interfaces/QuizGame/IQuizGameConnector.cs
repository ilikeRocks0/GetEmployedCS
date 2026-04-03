using Back_end.Objects;
namespace Back_end.Services.Interfaces;
public interface IQuizGameConnector
{
    public void InitializeSession(User user);

    public QuizItem? GetNextQuiz(User user);

    public void AnswerQuiz(User user, string answer);
    
    public QuizGameStats GetGameStats(User user);
}