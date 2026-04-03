using System.Text.Json;
using Back_end.Endpoints.Models;
using Back_end.Services.Implementations.AI.Prompts;
using Back_end.Services.Interfaces;

namespace Back_end.Services.Implementations.AI;

public class GroqGenericWordsService(IGroqService groqService) : IAiGenericWordsService
{
    public async Task<GenericWordsAnalysis> AnalyzeParagraph(string paragraph)
    {
        var userPrompt = GenericWordsPrompts.AnalyzeParagraph.Replace("<<PARAGRAPH>>", paragraph);
        var responseJson = await groqService.CompleteAsync(GenericWordsPrompts.SystemPrompt, userPrompt);

        using var doc = JsonDocument.Parse(responseJson);
        var root = doc.RootElement;

        return new GenericWordsAnalysis
        {
            Positions = [..root.GetProperty("positions")
                .EnumerateArray()
                .Select(e => e.GetInt32())],
                
            Advice = root.GetProperty("advice").GetString() ?? "",

            Recommendations = [..root.GetProperty("recommendations")
                .EnumerateArray()
                .Select(e => new WordRecommendation
            {
                Word = e.GetProperty("word").GetString() ?? "",
                Suggestion = e.GetProperty("suggestion").GetString() ?? ""
            })]
        };
    }
}
