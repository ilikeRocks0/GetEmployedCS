using Back_end.Endpoints.Models;
using Back_end.Services.Interfaces;

namespace Back_end.Endpoints;

public static class ResumeEndpoints
{
    public static void MapResumeEndpoints(this IEndpointRouteBuilder routes)
    {
        // This endpoint allows the user to provide a paragraph and retrieve the generic word positions in it using a base generic word list from the database.
        // The paragraph to analyze is extracted from the body into a GenericWordsRequest object automatically based on the definition of a GenericWordsRequest object.
        routes.MapPost("/api/resume/generic-words", (GenericWordsRequest request, IGenericWordsService genericWordsService) =>
        {
            return genericWordsService.GetPositionOfGenericWords(request.Paragraph);
        })
            .WithName("GetPositionOfGenericWords")
            .WithTags("Resume")
            .WithOpenApi()
            .RequireAuthorization();
    }
}