using System.Linq.Expressions;
using Back_end.Endpoints.Models;
using Back_end.Persistence.Implementations;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Implementations;
using Back_end.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace test;

public class QuizGameServiceIntegrationTest : IntegrationTest
{
    private IUserPersistence userPersistence;
    private IQuizItemsPersistence quizItemsPersistence;
    private IQuizGameService quizGameService;
    private User user;
    
    [SetUp]
    public override void Setup()
    {
        base.Setup();

        userPersistence = new UserPersistence(this.config, new PasswordHasher<User>());
        quizItemsPersistence = new QuizItemsPersisitence(this.config);
        quizGameService = new QuizGameService(userPersistence, quizItemsPersistence);

        user = new User(0, "email@email.com", "user", "pass", "about", "test", "user", []);
        user.UserId = userPersistence.CreateUser(user).userId;
    }

    [Test]
    public void RejectJobIntegrationTest()
    {
        CurrentUser currentUser = new CurrentUser(user.UserId);
        quizGameService.InitializeSession(currentUser);
        QuizItem? quiz = quizGameService.GetNextQuiz(currentUser);
        Assert.That(quiz, Is.Null);
    }
}