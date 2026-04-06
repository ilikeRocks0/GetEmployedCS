using Back_end.Persistence.Interfaces;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations;

public class RandomQuizItemFetcherFactory(IQuizItemsPersistence quizItemsPersistence) : IQuizItemFetcherFactory
{
    private IQuizItemsPersistence quizItemsPersistence = quizItemsPersistence;

    /// Allows code to send spies to IQuizGameConnector. 
    /// Returns a quiz item fetcher. 
    public IQuizItemFetcher BuildFetcher()
    {
        return new QuizItemRandomFetcher(quizItemsPersistence);
    }
}