using Back_end.Endpoints.Models;
using Back_end.Services.Interfaces;

namespace Back_end.Endpoints;

public static class GenericWordEndpoints
{
    public static void MapGenericWordEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/genericWord", (GenericWords genericWords, HttpContext context, IGenericWordsService genericWordsService) =>
        {
            return genericWordsService.GetPositionOfGenericWords(genericWords.GenericWord);
        })
            .WithName("FetchWordPositions")
            .WithTags("GenericWord")
            .WithOpenApi()
            .RequireAuthorization();
    }
}