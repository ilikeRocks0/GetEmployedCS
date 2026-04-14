namespace test;

using Back_end.Persistence.Implementations;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Implementations;
using Back_end.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using Back_end.Endpoints.Models;

public class FollowingServiceIntegrationTest : IntegrationTest
{
    private IFollowService followService;
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
        followService = new FollowService(userPersistence);

        user.UserId = userPersistence.CreateUser(user).userId;
        user2.UserId = userPersistence.CreateUser(user2).userId;
    }

    [Test]
    public void FollowUserIntegrationTest()
    {
        Assert.That(followService.IsFollowing(user.UserId, user2.Username), Is.EqualTo(false));
        followService.FollowUser(user.UserId, user2.Username);
        Assert.That(followService.IsFollowing(user.UserId, user2.Username), Is.EqualTo(true));      
    }
}