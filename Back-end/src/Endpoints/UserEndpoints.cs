using Back_end.Services.Interfaces;
using Back_end.Endpoints.Models;
using Back_end.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Back_end.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/users", (NewUser newUser, IUserService userService) =>
        {
            return userService.CreateUser(newUser);
        })
            .WithName("CreateUser")
            .WithTags("Users")
            .WithOpenApi();

        routes.MapPost("/api/users/save", (HttpContext context, IUserService userService) =>
        {
            var filters = context.Request.Query.ToDictionary(query => query.Key, query => query.Value.ToString());
            var userId = context.User.FindFirst("UserId")?.Value;
            filters[AppConfig.FilterKeys.USERID] = userId ?? "0";
            return userService.SaveJob(filters.Count > 0 ? filters : null);
        })
            .WithName("SaveJob")
            .WithTags("Users")
            .WithOpenApi()
            .RequireAuthorization();


         routes.MapPost("/api/users/unsave", (HttpContext context, IUserService userService) =>
        {
            var filters = context.Request.Query.ToDictionary(query => query.Key, query => query.Value.ToString());
            var userId = context.User.FindFirst("UserId")?.Value;
            filters[AppConfig.FilterKeys.USERID] = userId ?? "0";
            return userService.UnsaveJob(filters.Count > 0 ? filters : null);
        })
            .WithName("UnsaveJob")
            .WithTags("Users")
            .WithOpenApi()
            .RequireAuthorization();

         routes.MapGet("/api/users/{username}", (string username, IUserService userService) =>
        {          

            return userService.GetProfileByUsername(username); 
        })
            .WithName("GetProfile")
            .WithTags("Users")
            .WithOpenApi()
            .RequireAuthorization();

        routes.MapPost("/api/users/login", async (LoginRequest loginRequest, HttpContext context, IUserService userService) =>
        {
            int userId;
            try
            {
                userId = userService.Login(loginRequest);
            }
            catch (InvalidOperationException)
            {
                return Results.Unauthorized();
            }

            if (userId <= 0)
            {
                return Results.Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim("UserId", userId.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
            };

            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            var profile = userService.GetProfile(userId);
            return Results.Ok(new
            {
                firstName = profile!.FirstName,
                lastName = profile.LastName,
                username = profile.Username,
                email = profile.Email,
            });
        })
            .WithName("Login")
            .WithTags("Users")
            .WithOpenApi();

        routes.MapPost("/api/users/logout", async (HttpContext context) =>
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.Ok();
        })
            .WithName("Logout")
            .WithTags("Users")
            .WithOpenApi();

        routes.MapGet("/api/users/check-employer", (HttpContext context, IUserService userService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();
            return Results.Ok(userService.CheckUserEmployer(userId));
        })
            .WithName("CheckUserEmployer")
            .WithTags("Users")
            .WithOpenApi()
            .RequireAuthorization();
    }

    public static void MapUserGameEndpoints(this IEndpointRouteBuilder routes)
    {
        // This endpoint initializes the job list for the game based on the provided filters and returns a random job to start the game. 
        // The filters are the same as those used in GetJobs.
        routes.MapPost("/api/users/game", (HttpContext context, IUserGameService userGameService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();
            return Results.Ok(userGameService.InitializeUserGame(new CurrentUser(userId)));
        })
            .WithName("InitializeUserGame")
            .WithTags("User Game")
            .WithOpenApi()
            .RequireAuthorization();

        // this endpoint allows the user to reject the current job in the game and receive the next job.
        routes.MapPost("/api/users/game/reject/{username}", (string username, HttpContext context, IUserGameService userGameService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();
            return Results.Ok(userGameService.RejectUser(new CurrentUser(userId), username));
        })
            .WithName("RejectUser")
            .WithTags("User Game")
            .WithOpenApi()
            .RequireAuthorization();

        // this endpoint allows the user to accept the current job in the game and receive the next job.
        routes.MapPost("/api/users/game/accept/{username}", (string username, HttpContext context, IUserGameService userGameService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();
            return Results.Ok(userGameService.AcceptUser(new CurrentUser(userId), username));
        })
            .WithName("AcceptUser")
            .WithTags("User Game")
            .WithOpenApi()
            .RequireAuthorization();

        // this endpoint allows the user to retrieve the current game statistics, including the number of accepted and rejected jobs.
        routes.MapPost("/api/users/game/stats", (HttpContext context, IUserGameService userGameService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();
            return Results.Ok(userGameService.GetGameStats(new CurrentUser(userId)));
        })
            .WithName("GetUserGameStats")
            .WithTags("User Game")
            .WithOpenApi()
            .RequireAuthorization();
    }
}