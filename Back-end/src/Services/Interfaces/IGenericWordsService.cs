namespace Back_end.Services.Interfaces;
public interface IGenericWordsService
{
    /// <summary>Extracts and returns a list of what position the generic words are in the paragraph. first word is 0, second word is 1, etc.</summary>
    List<int> GetPositionOfGenericWords(string Paragraph);
}