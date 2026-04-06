using Back_end.Services.Interfaces;
using Back_end.Persistence.Interfaces;

namespace Back_end.Services.Implementations;

public class GenericWordsService (IResumePersistence resumePersistence): IGenericWordsService
{
    /// Extracts and returns a list of what position the generic words are in the paragraph. first word is 0, second word is 1, etc.
    /// <param name="paragraph">The paragraph to analyze for generic words.
    /// Returns a list of int indexes where generic words in the paragraph are. 
    public List<int> GetPositionOfGenericWords(string Paragraph)
    {
        var genericWords = new HashSet<string>(resumePersistence.GetGenericWords(), StringComparer.OrdinalIgnoreCase);
        List<int> positions = [];
        var words = Paragraph.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        char[] punctuationToTrim = ['.', ',', '!', '?', ';', ':', '(', ')', '[', ']', '{', '}', '"', '\'', '“', '”'];
        
        for (int i = 0; i < words.Length; i++)
        {
            string cleanWord = words[i].Trim(punctuationToTrim);
            if (genericWords.Contains(cleanWord))
            {
                positions.Add(i);
            }
        }
        return positions;
    }
}