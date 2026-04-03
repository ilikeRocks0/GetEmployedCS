using System.Data.Entity;
using Back_end.Persistence.Implementations.Adapters.EntityAdapters;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;

namespace Back_end.Persistence.Implementations;

public class QuizItemsPersisitence : IQuizItemsPersistence
{
    private IConfiguration config;

    public QuizItemsPersisitence(IConfiguration config)
    {
        this.config = config;
    }

    public List<QuizItem> GetQuizItems(int max)
    {
        using (AppDbContext context = new(this.config))
        {
            return context.QuizItems
                .Take(max)
                .Select(quizItemEntity => new QuizItemEntityAdapter(quizItemEntity))
                .ToList<QuizItem>();
        }
    }
}