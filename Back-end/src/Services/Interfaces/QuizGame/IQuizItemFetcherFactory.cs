using Back_end.Services.Interfaces;

namespace Back_end.Services.Interfaces;
//Returns a quiz item fetcher
//this allows us to send spys to the IQuizGameConnector
public interface IQuizItemFetcherFactory
{
    public IQuizItemFetcher BuildFetcher();
}