using Back_end.Services.Interfaces;
namespace Back_end.Endpoints;


public static class CommentEndpoints
{
    public static void MapCommentsEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/comments/create", (NewJobComment comment, ICommentsService commentsService) =>
        {
            return commentsService.CreateComment(comment);
        })
            .WithName("CreateComment")
            .WithTags("Comments")
            .WithOpenApi()
            .RequireAuthorization();

        routes.MapGet("/api/comments/{jobId}", (int jobId, ICommentsService commentsService) =>
        {            
            return commentsService.GetComments(jobId);
        })
            .WithName("GetComments")
            .WithTags("Comments")
            .WithOpenApi()
            .RequireAuthorization();
    }
}