using Back_end.Endpoints.Models;
using Back_end.Services.Interfaces;
using Back_end.Util;


namespace Back_end.Endpoints;

public static class QuizGameEndPoints
{
    public static void MapQuizGameEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/quiz/game", (HttpContext context, IQuizGameService quizGameService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();

            quizGameService.InitializeSession(new CurrentUser(userId));            
            return Results.Ok();
        })
            .WithName("InitializeQuizGame")
            .WithTags("Quiz Game")
            .WithOpenApi()
            .RequireAuthorization();

        routes.MapPost("/api/quiz/answer", (QuizGameResponse quizGameResponse, HttpContext context, IQuizGameService quizGameService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();

            quizGameService.AnswerQuiz(new CurrentUser(userId), quizGameResponse);            
            return Results.Ok();
        })
            .WithName("AnswerQuiz")
            .WithTags("Quiz Game")
            .WithOpenApi()
            .RequireAuthorization();

        routes.MapPost("/api/quiz/stats", (HttpContext context, IQuizGameService quizGameService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();

            return Results.Ok(quizGameService.GetGameStats(new CurrentUser(userId)));
        })
            .WithName("GetQuizStats")
            .WithTags("Quiz Game")
            .WithOpenApi()
            .RequireAuthorization();

        routes.MapPost("/api/quiz/next", (HttpContext context, IQuizGameService quizGameService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();

            return Results.Ok(quizGameService.GetNextQuiz(new CurrentUser(userId)));
        })
            .WithName("GetNextQuiz")
            .WithTags("Quiz Game")
            .WithOpenApi()
            .RequireAuthorization();
    }
}