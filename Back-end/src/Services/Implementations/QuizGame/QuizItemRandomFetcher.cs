//Gets a list of quiz items that is randomly fetched
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Interfaces;
using Back_end.Util;

namespace Back_end.Services.Implementations;

public class QuizItemRandomFetcher : IQuizItemFetcher
{
    Queue<QuizItem> quizItems = [];

    public QuizItemRandomFetcher(IQuizItemsPersistence quizItemsPersistence)
    {
        List<QuizItem> quizes = quizItemsPersistence.GetQuizItems(AppConfig.QUIZ_ITEM_AMOUNT);
        QuizItem[] shuffled = quizes.ToArray();
        Random.Shared.Shuffle(shuffled);  
        quizItems = new Queue<QuizItem>(shuffled.ToList());
    }

    public QuizItem GetQuizItem()
    {
        if (quizItems.Count() <= 0)
        {
            throw new InvalidOperationException("No more Quiz Items"); 
        }

        return quizItems.Dequeue();
    }
}