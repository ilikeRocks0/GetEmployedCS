using System.Linq.Expressions;
using Back_end.Endpoints.Models;
using Back_end.Persistence.Implementations;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Implementations;
using Back_end.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace test;

public class UserGameServiceIntegrationTest : IntegrationTest
{
    private IUserPersistence userPersistence;
    private IUserGameService userGameService;
    private User user;
    
    [SetUp]
    public override void Setup()
    {
        base.Setup();

        user = new User(0, "email1@email.com", "user", "pass", "about", "test", "user", []);
        User user2 = new User(1, "second2@email.com", "user2", "pass2", "about2", "test2", "user2", []);
        userPersistence = new UserPersistence(this.config, new PasswordHasher<User>());

        user.UserId = userPersistence.CreateUser(user).userId;
        userPersistence.CreateUser(user2);
        userGameService = new UserGameService(userPersistence);
    }

    [Test]
    public void RejectUserIntegrationTest()
    {
        CurrentUser currentUser = new CurrentUser(user.UserId);
        Profile? next = userGameService.InitializeUserGame(currentUser);

        Assert.That(next is not null);

        CurrentUser userNext = new CurrentUser(next!.UserId);

        Profile? next2 = userGameService.RejectUser(currentUser, next.Username);
        (int accepted, int rejected) = userGameService.GetGameStats(currentUser);

        Assert.Multiple(() =>
        {
            Assert.That(accepted, Is.EqualTo(0));
            Assert.That(rejected, Is.EqualTo(1));
        });
    }

    [Test]
    public void AcceptUserIntegrationTest()
    {
        CurrentUser currentUser = new CurrentUser(user.UserId);
        Profile? next = userGameService.InitializeUserGame(currentUser);

        Assert.That(next is not null);

        CurrentUser userNext = new CurrentUser(next!.UserId);

        Profile? next2 = userGameService.AcceptUser(currentUser, next.Username);
        (int accepted, int rejected) = userGameService.GetGameStats(currentUser);

        Assert.Multiple(() =>
        {
            Assert.That(accepted, Is.EqualTo(1));
            Assert.That(rejected, Is.EqualTo(0));
        });
    }
}