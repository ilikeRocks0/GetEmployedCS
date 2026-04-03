using Back_end.Endpoints.Models;
using Back_end.Services.Interfaces;

namespace Back_end.Endpoints;

public static class GenericWordEndpoints
{
    public static void MapGenericWordEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/genericWord", (GenericWords genericWords, IGenericWordsService genericWordsService) =>
        {
            return genericWordsService.GetPositionOfGenericWords(genericWords.GenericWord);
        })
            .WithName("FetchWordPositions")
            .WithTags("GenericWord")
            .WithOpenApi()
            .RequireAuthorization();

        routes.MapPost("/api/genericWord/analyze", async (GenericWords genericWords, IAiGenericWordsService aiGenericWordsService) =>
        {
            return await aiGenericWordsService.AnalyzeParagraph(genericWords.GenericWord);
        })
            .WithName("AnalyzeWordPositions")
            .WithTags("GenericWord")
            .WithOpenApi()
            .RequireAuthorization();

        routes.MapGet("/api/genericWord/status", (IGroqService groqService) =>
            Results.Ok(new { groqAvailable = groqService.IsAvailable }))
            .WithName("GroqStatus")
            .WithTags("GenericWord")
            .WithOpenApi()
            .RequireAuthorization();
    }
}