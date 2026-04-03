using Back_end.Endpoints.Models;

namespace Back_end.Services.Interfaces;

public interface IAiGenericWordsService
{
    /// <summary>Processes the paragraph input given by the user in the Generic Word Detector.</summary>
    /// <returns>A JSON response containing positions of the words in the string, 
    /// advice, and the detected word followed by a recommended replacement.</returns>
    Task<GenericWordsAnalysis> AnalyzeParagraph(string paragraph);
}
