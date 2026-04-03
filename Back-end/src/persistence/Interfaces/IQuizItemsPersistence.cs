using Back_end.Objects;

namespace Back_end.Persistence.Interfaces;

public interface IQuizItemsPersistence
{
    /// <summary>
    /// Gets a list of Quiz Items 
    /// </summary>
    /// <param name="max">the max amount gotten</param>
    /// <returns>a list of Quiz Items</returns>
    List<QuizItem> GetQuizItems(int max);

}