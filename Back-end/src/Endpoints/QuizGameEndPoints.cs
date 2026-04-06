using Back_end.Endpoints.Models;
using Back_end.Objects;
using Back_end.Services.Interfaces;
using Back_end.Util;

namespace Back_end.Endpoints;

public static class QuizGameEndPoints
{
    public static void MapQuizGameEndpoints(this IEndpointRouteBuilder routes)
    {
        // This endpoint initializes the session for the game.
        // UserId is extracted from the user's cookie. This is required to identify the user to connect the game's instance to.
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

        // This endpoint accepts the user's selected sentence answer from the game.
        // UserId is extracted from the user's cookie. This is required to identify the user to connect the game's instance to.
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

        // This endpoint allows the user to retrieve the current game statistics, including the number of correct, incorrect and skipped sentences.
        // UserId is extracted from the user's cookie. This is required to identify the user to connect the game's instance to.
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

        // This endpoint provides the next pair of sentences for the game.
        // UserId is extracted from the user's cookie. This is required to identify the user to connect the game's instance to.
        routes.MapPost("/api/quiz/next", (HttpContext context, IQuizGameService quizGameService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();
            QuizItem? quizItem = quizGameService.GetNextQuiz(new CurrentUser(userId));
            if (quizItem == null)
            {
                return Results.Ok();
            }
            return Results.Ok(new QuizOptions(quizItem));
        })
            .WithName("GetNextQuiz")
            .WithTags("Quiz Game")
            .WithOpenApi()
            .RequireAuthorization();
    }
}