using Back_end.Objects;

namespace Back_end.Services.Interfaces;

public interface IQuizItemFetcher
{
    /// Get a quiz item for the quiz game instance.
    /// Returns 1 quiz item at a time
    public QuizItem GetQuizItem();
}