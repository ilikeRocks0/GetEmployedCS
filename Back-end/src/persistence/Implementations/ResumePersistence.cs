using Back_end.Persistence.Interfaces;

namespace Back_end.Persistence.Implementations;

public class ResumePersistence : IResumePersistence
{
    private IConfiguration config;

    public ResumePersistence(IConfiguration config)
    {
        this.config = config;
    }

    public List<string> GetGenericWords()
    {
        List<string> words = new();

        using (AppDbContext context = new(this.config))
        {
            context.GenericWords.ToList().ForEach(e =>
            {
                words.Add(e.word);
            });
        }

        return words;
    }
}