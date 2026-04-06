using Back_end.Services.Interfaces;
using Back_end.Endpoints.Models;
using Back_end.Objects;
using Back_end.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Back_end.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        // This endpoint retrieves a list of users based on the provided filters.
        // The filters are passed as query parameters.
        routes.MapGet("/api/users/search", (HttpContext context, IUserService userService) =>
        {
            var filters = context.Request.Query.ToDictionary(query => query.Key, query => query.Value.ToString());
            return userService.GetUsers(filters.Count > 0 ? filters : null);
        })
            .WithName("SearchUsers")
            .WithTags("Users")
            .WithOpenApi()
            .RequireAuthorization();

        // This endpoint creates a new user account.
        // User information is extracted from the body into a NewUser object automatically based on the definition of a NewUser object.
        routes.MapPost("/api/users", async (NewUser newUser, IUserService userService) =>
        {
            return await userService.CreateUser(newUser);
        })
            .WithName("CreateUser")
            .WithTags("Users")
            .WithOpenApi();

        // This endpoint saves a job for the user. 
        // Filters contain the jobId of the job to be saved. 
        // UserId is extracted from the user's cookie. This is required to save the job for the appropriate user.
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

        // This endpoint unsaves a job for the user; that is, removes the job from the user's saved jobs.
        // Filters contain the jobId of the job to be unsaved. 
        // UserId is extracted from the user's cookie. This is required to unsave the job for the appropriate user.
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

        // This endpoint retrieves the profile details of a specified user.
        // A username for the target user to retrieve is passed in as part of the URI. 
         routes.MapGet("/api/users/{username}", (string username, HttpContext context, IUserService userService, IFollowService followService) =>
        {
            var profile = userService.GetProfileByUsername(username);
            if (profile is null) return Results.NotFound();
            bool isSelf = int.TryParse(context.User.FindFirst("UserId")?.Value, out var requestingUserId)
                && profile.UserId == requestingUserId;
            bool isFollowing = !isSelf && requestingUserId != 0 && followService.IsFollowing(requestingUserId, username);

            return Results.Ok(new {
                profile.UserId,
                profile.Username,
                profile.Email,
                profile.FirstName,
                profile.LastName,
                profile.Bio,
                profile.Experiences,
                profile.IsEmployer,
                profile.EmployerName,
                profile.PostedJobs,
                isSelf,
                isFollowing
            });
        })
            .WithName("GetProfile")
            .WithTags("Users")
            .WithOpenApi()
            .RequireAuthorization();

        // This endpoint logs the user in and retrieves the profile for their account, if appropriate credentials were provided.
        // Login credentials are extracted from the body into a LoginRequest object automatically based on the definition of a LoginRequest object.
        // Upon a successful login, the user receives a cookie to allow them to remain logged in for some time. 
        // The cookie's timeout refreshes on all other endpoint calls, except for the Logout endpoint below. 
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

        // This endpoint logs the user out. 
        // This invalidates and clears the user's cookie. 
        routes.MapPost("/api/users/logout", async (HttpContext context) =>
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            context.Response.Headers.Append("Clear-Site-Data", "\"cookies\", \"storage\", \"cache\"");

            return Results.Ok();
        })
            .WithName("Logout")
            .WithTags("Users")
            .WithOpenApi();
        
        // This endpoint retrieves the list of accounts a user follows.
        // UserId is extracted from the user's cookie. This is required to retrieve the following list for the appropriate user.
        routes.MapGet("/api/users/following", (HttpContext context, IUserService userService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();
            return Results.Ok(userService.GetAllFollowing(userId));
        })
            .WithName("GetFollowing")
            .WithTags("Users")
            .WithOpenApi()
            .RequireAuthorization();

        // This endpoint verifies if the current user is an employer or not.
        // UserId is extracted from the user's cookie. This is required to check the user type (employer or job seeker) for the appropriate user.
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

        // This endpoint updates the current user's account. 
        // User account details are extracted from the body into an UpdateUserRequest object automatically based on the definition of a UpdateUserRequest object.
        // UserId is extracted from the user's cookie. This is required to connect to the appropriate user to update.
        routes.MapPut("/api/users", (UpdateUserRequest request, HttpContext context, IUserService userService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();
            return Results.Ok(userService.UpdateUser(request, userId));
        })
            .WithName("UpdateUser")
            .WithTags("Users")
            .WithOpenApi()
            .RequireAuthorization();

        // This endpoint adds a new experience to the user's profile. 
        // Experience details are extracted from the body into an Experience object automatically based on the definition of a Experience object.
        // UserId is extracted from the user's cookie. This is required to connect to the user as the one with the experience.
        routes.MapPost("/api/users/experiences", (Experience experience, HttpContext context, IUserService userService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();
            try
            {
                var experienceId = userService.AddExperience(userId, experience);
                return Results.Ok(experienceId);
            }
            catch (InvalidOperationException)
            {
                return Results.Forbid();
            }
        })
            .WithName("AddExperience")
            .WithTags("Users")
            .WithOpenApi()
            .RequireAuthorization();

        // This endpoint updates an existing experience on the user's profile. 
        // Experience details are extracted from the body into an UpdateExperienceRequest object automatically based on the definition of a UpdateExperienceRequest object.
        // UserId is extracted from the user's cookie. This is required to connect to the user and update the appropriate experience.
        routes.MapPut("/api/users/experiences", (UpdateExperienceRequest request, HttpContext context, IUserService userService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();
            try
            {
                userService.EditExperience(userId, request.OldExperience, request.NewExperience);
                return Results.Ok();
            }
            catch (InvalidOperationException)
            {
                return Results.Forbid();
            }
        })
            .WithName("EditExperience")
            .WithTags("Users")
            .WithOpenApi()
            .RequireAuthorization();

        // This endpoint removes an experience from the user's profile. 
        // Experience details are extracted from the body into an Experience object automatically based on the definition of a Experience object.
        // UserId is extracted from the user's cookie. This is required to connect to the user and delete the appropriate experience.
        routes.MapDelete("/api/users/experiences", ([FromBody] Experience experience, HttpContext context, IUserService userService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();
            try
            {
                userService.DeleteExperience(userId, experience);
                return Results.Ok();
            }
            catch (InvalidOperationException)
            {
                return Results.Forbid();
            }
        })
            .WithName("DeleteExperience")
            .WithTags("Users")
            .WithOpenApi()
            .RequireAuthorization();


        routes.MapGet("/api/users/verify", (string token, IUserService userService) =>
        {
            try
            {
                userService.VerifyUser(token);
                return Results.Ok();
            }
            catch (InvalidOperationException)
            {
                return Results.BadRequest("Invalid or already-used verification token.");
            }
        })
            .WithName("VerifyUser")
            .WithTags("Users")
            .WithOpenApi();
        // This endpoint adds a user to the current user's following list. 
        // The username of the user to follow is automatically extracted from the URI into a username string.
        // UserId is extracted from the user's cookie. This is required to identify the user to update the following list of.
        routes.MapPost("/api/users/follow/{username}", (string username, HttpContext context, IFollowService followService) =>
        {
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();
            try
            {
                followService.FollowUser(userId, username);
                return Results.Ok();
            }
            catch (InvalidOperationException)
            {
                return Results.Forbid();
            }
        })
            .WithName("FollowUser")
            .WithTags("Users")
            .WithOpenApi()
            .RequireAuthorization();
        
        // This endpoint removes a user from the current user's following list. 
        // The username of the user to unfollow is automatically extracted from the URI into a username string.
        // UserId is extracted from the user's cookie. This is required to identify the user to update the following list of.
        routes.MapPost("/api/users/unfollow/{username}", (string username, HttpContext context, IFollowService followService) =>
        {            
            var userIdStr = context.User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();
            try
            {                
                followService.UnfollowUser(userId, username);
                return Results.Ok();
            }
            catch (InvalidOperationException)
            {
                return Results.Forbid();
            }
        })
            .WithName("UnfollowUser")
            .WithTags("Users")
            .WithOpenApi()
            .RequireAuthorization();
    }

    public static void MapUserGameEndpoints(this IEndpointRouteBuilder routes)
    {
        // This endpoint initializes the user list for the game and returns a random user to start the game. 
        // UserId is extracted from the user's cookie. This is required to identify the user to connect the game's instance to.
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

        // This endpoint allows the user to reject the current user in the game and receive the next user.
        // UserId is extracted from the user's cookie. This is required to identify the user to connect the game's instance to.
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

        // This endpoint allows the user to accept the current job in the game and receive the next job.
        // UserId is extracted from the user's cookie. This is required to identify the user to connect the game's instance to.
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
        
        // This endpoint allows the user to retrieve the current game statistics, including the number of accepted and rejected users.
        // UserId is extracted from the user's cookie. This is required to identify the user to connect the game's instance to.
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