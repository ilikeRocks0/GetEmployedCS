using Back_end.Endpoints.Models;
using Back_end.Services.Interfaces;
namespace Back_end.Endpoints;

public static class CommentEndpoints
{
    public static void MapCommentsEndpoints(this IEndpointRouteBuilder routes)
    {
        // This endpoint allows the user to add a new comment to a job. 
        // Comment details are extracted from the body into a NewJobComment object automatically based on the definition of a NewJobComment object.
        // UserId is extracted from the user's cookie. This is required to connect the new comment to the user as its poster. 
        routes.MapPost("/api/comments/create", (NewJobComment comment, HttpContext context, ICommentsService commentsService) =>
        {
            var userId = context.User.FindFirst("UserId")?.Value;
            comment.PosterUserId = int.TryParse(userId, out var id) ? id : 0;
            return commentsService.CreateComment(comment);
        })
            .WithName("CreateComment")
            .WithTags("Comments")
            .WithOpenApi()
            .RequireAuthorization();

        // This endpoint retrieves the comments of a specified job.
        // A jobId for the target job to get comments for is passed in as part of the URI. 
        routes.MapGet("/api/comments/{jobId}", (int jobId, ICommentsService commentsService) =>
        {            
            return commentsService.GetComments(jobId);
        })
            .WithName("GetComments")
            .WithTags("Comments")
            .WithOpenApi()
            .RequireAuthorization();
        
        // This endpoint allows the user to add a new comment to a user's profile. 
        // Comment details are extracted from the body into a NewUserComment object automatically based on the definition of a NewUserComment object.
        // UserId is extracted from the user's cookie. This is required to connect the new comment to the user as its poster. 
        routes.MapPost("/api/usercomments/create", (NewUserComment comment, HttpContext context, IUserCommentsService userCommentsService) =>
        {
            var userId = context.User.FindFirst("UserId")?.Value;
            comment.PosterUserId = int.TryParse(userId, out var id) ? id : 0;
            return userCommentsService.CreateComment(comment);
        })
            .WithName("CreateUserComment")
            .WithTags("UserComments")
            .WithOpenApi()
            .RequireAuthorization();
        
        routes.MapPost("/api/usercomments/notify", async (NewUserComment comment, HttpContext context, IUserService userService, IEmailService emailService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var posterId))
                return Results.Unauthorized();

            var posterProfile = userService.GetProfile(posterId);
            var profileOwner = userService.GetProfileByUsername(comment.CommentedUserUsername);

            if (posterProfile is null || profileOwner is null)
                return Results.NotFound();

            await emailService.SendProfileCommentNotificationAsync(profileOwner.Email, posterProfile.Username, comment.CommentedUserUsername, comment.Comment);

            return Results.Ok();
        })
            .WithName("NotifyProfileComment")
            .WithTags("UserComments")
            .WithOpenApi()
            .RequireAuthorization();

        // This endpoint retrieves the comments of a specified user profile.
        // A username for the target user to get comments for is passed in as part of the URI. 
        routes.MapGet("/api/usercomments/{username}", (string username, IUserCommentsService userCommentsService) =>
        {
            return userCommentsService.GetComments(username);
        })
            .WithName("GetUserComments")
            .WithTags("UserComments")
            .WithOpenApi()
            .RequireAuthorization();
    }
}