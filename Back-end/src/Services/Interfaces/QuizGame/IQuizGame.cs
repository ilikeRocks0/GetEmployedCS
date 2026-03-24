namespace Back_end.Services.Interfaces;
public interface IQuizGame
{
    //returns null when out of questions
    public QuizItem? GetNextQuiz();

    //Answer the current question
    public void AnswerQuiz(string answer);

    //returns the amount skipped, correct, and incorrect
    public QuizGameStats GetQuizGameStats();
}