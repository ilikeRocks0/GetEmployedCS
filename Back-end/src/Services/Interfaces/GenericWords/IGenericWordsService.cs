namespace Back_end.Services.Interfaces;

public interface IGenericWordsService
{
    /// Extracts and returns a list of what position the generic words are in the paragraph. first word is 0, second word is 1, etc.
    /// <param name="paragraph">The paragraph to analyze for generic words.
    /// Returns a list of int indexes where generic words in the paragraph are. 
    List<int> GetPositionOfGenericWords(string Paragraph);
}