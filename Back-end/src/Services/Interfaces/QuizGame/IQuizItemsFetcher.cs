using Back_end.Objects;

namespace Back_end.Services.Interfaces;

//Provides a simple interface to fetch quiz items
public interface IQuizItemFetcher
{
    //returns 1 quiz at a time
    public QuizItem GetQuizItem();
}