using Back_end.Endpoints.Models;
using Back_end.Services.Interfaces;

namespace Back_end.Endpoints;

public static class ResumeEndpoints
{
    public static void MapResumeEndpoints(this IEndpointRouteBuilder routes)
    {
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