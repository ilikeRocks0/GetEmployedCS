using Back_end.Objects;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class QuizGame : IQuizGame
{
    private IQuizItemFetcher quizItemFetcher;
    private QuizItem? current = null;
    private QuizGameStats quizGameStats;
    public QuizGame(IQuizItemFetcher quizItemFetcher)
    {
        this.quizItemFetcher = quizItemFetcher;
        quizGameStats = new QuizGameStats();
    }
    public void AnswerQuiz(string answer)
    {
        if (current == null)
        {
            throw new InvalidOperationException("Trying to answer quiz when no question ready!");
        }

        if (current.strongSentence.Equals(answer))
        {
            quizGameStats.Correct++;
            current = null;
        }
        else if (current.weakSentence.Equals(answer))
        {
            quizGameStats.Incorrect++;
            current = null;
        }
        else
        {
            throw new InvalidOperationException("Provided an answer that is not avaliable!");
        }
    }

    public QuizItem? GetNextQuiz()
    {
        try
        {
            if(current != null)
            {
                quizGameStats.Skipped++;
            }

            current = quizItemFetcher.GetQuizItem();
        }
        catch (System.Exception)
        {
            return null;
        }
        return current;
    }

    public QuizGameStats GetQuizGameStats()
    {
        return quizGameStats;
    }
}