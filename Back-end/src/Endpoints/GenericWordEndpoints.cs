using Back_end.Endpoints.Models;
using Back_end.Services.Interfaces;

namespace Back_end.Endpoints;

public static class GenericWordEndpoints
{
    public static void MapGenericWordEndpoints(this IEndpointRouteBuilder routes)
    {
        // This endpoint allows the user to provide a paragraph and retrieve the generic word positions in it using a base generic word list from the database.
        // The paragraph to analyze is extracted from the body into a GenericWords object automatically based on the definition of a GenericWords object.
        routes.MapPost("/api/genericWord", (GenericWords genericWords, IGenericWordsService genericWordsService) =>
        {
            return genericWordsService.GetPositionOfGenericWords(genericWords.GenericWord);
        })
            .WithName("FetchWordPositions")
            .WithTags("GenericWord")
            .WithOpenApi()
            .RequireAuthorization();

        // This endpoint allows the user to provide a paragraph and retrieve the generic word positions in it using an external API to an AI agent.
        // The paragraph to analyze is extracted from the body into a GenericWords object automatically based on the definition of a GenericWords object.
        routes.MapPost("/api/genericWord/analyze", async (GenericWords genericWords, IAiGenericWordsService aiGenericWordsService) =>
        {
            return await aiGenericWordsService.AnalyzeParagraph(genericWords.GenericWord);
        })
            .WithName("AnalyzeWordPositions")
            .WithTags("GenericWord")
            .WithOpenApi()
            .RequireAuthorization();

        // This endpoint checks if Groq, an API to an AI agent, is available to use. 
        routes.MapGet("/api/genericWord/status", (IGroqService groqService) =>
            Results.Ok(new { groqAvailable = groqService.IsAvailable }))
            .WithName("GroqStatus")
            .WithTags("GenericWord")
            .WithOpenApi()
            .RequireAuthorization();
    }
}