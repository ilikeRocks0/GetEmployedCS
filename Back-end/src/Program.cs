using DotNetEnv;
using Back_end.Endpoints;
using Back_end.Persistence.Implementations;
using Back_end.Persistence.Objects;
using Back_end.Persistence.Interfaces;
using Back_end.Services.Interfaces;
using Back_end.Services.Implementations;
using Microsoft.AspNetCore.Authentication.Cookies;
// Load environment variables from .env file
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.AllowTrailingCommas = true;
});
builder.Services.AddScoped<IJobPersistence, JobPersistence>();
builder.Services.AddScoped<IQuizItemsPersistence, QuizItemsPersisitence>();
builder.Services.AddScoped<IJobIndexManager, ShuffleJobsService>();
builder.Services.AddScoped<IResumePersistence, ResumePersistence>();
builder.Services.AddScoped<IGenericWordsService, GenericWordsService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IJobAddService, JobAddService>();
builder.Services.AddSingleton<IJobGameService, GameServiceSingleton>();
builder.Services.AddSingleton<IUserGameService, UserGameSingleton>();
builder.Services.AddSingleton<IQuizGameService, QuizGameSingleton>();
builder.Services.AddScoped<IUserPersistence, UserPersistence>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICommentsService, CommentsService>();
builder.Services.AddScoped<Microsoft.AspNetCore.Identity.IPasswordHasher<User>, Microsoft.AspNetCore.Identity.PasswordHasher<User>>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.Events.OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapJobEndpoints();
app.MapJobGameEndpoints();
app.MapJobCreationEndpoints();
app.MapUserEndpoints();
app.MapUserGameEndpoints();
app.MapCommentsEndpoints();
app.MapQuizGameEndpoints();
app.MapGenericWordEndpoints();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
