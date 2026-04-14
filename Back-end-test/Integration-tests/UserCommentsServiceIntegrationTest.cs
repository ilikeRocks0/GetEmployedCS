namespace test;

using Back_end.Persistence.Implementations;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Implementations;
using Back_end.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using Back_end.Endpoints.Models;

public class UserCommentsServiceIntegrationTest : IntegrationTest
{
    private IUserCommentsService userCommentsService;
    private IUserPersistence userPersistence;
    private User user;
    private User user2;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        user = new User(0, "email1@email.com", "user", "pass", "about", "test", "user", []);
        user2 = new User(1, "second2@email.com", "user2", "pass2", "about2", "test2", "user2", []);
        
        userPersistence = new UserPersistence(this.config, new PasswordHasher<User>());
        userCommentsService = new UserCommentsService(userPersistence);
        
        user.UserId = userPersistence.CreateUser(user).userId;
        user2.UserId = userPersistence.CreateUser(user2).userId;
    }

    [Test]
    public void CreateCommentIntegrationTest()
    {
        List<UserComment> comments = userCommentsService.GetComments(user2.Username);
        Assert.That(comments, Is.Empty);
        NewUserComment userComment = new NewUserComment("cool guy!", user.UserId, user2.Username);
        userCommentsService.CreateComment(userComment);
        List<UserComment> comments2 = userCommentsService.GetComments(user2.Username);
        Assert.That(comments2, Is.Not.Empty);
    }
}