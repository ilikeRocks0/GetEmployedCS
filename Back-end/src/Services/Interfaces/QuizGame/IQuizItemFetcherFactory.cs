using Back_end.Services.Interfaces;

namespace Back_end.Services.Interfaces;

public interface IQuizItemFetcherFactory
{
    /// Allows code to send spies to IQuizGameConnector. 
    /// Returns a quiz item fetcher. 
    public IQuizItemFetcher BuildFetcher();
}