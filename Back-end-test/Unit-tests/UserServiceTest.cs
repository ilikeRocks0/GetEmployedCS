using NSubstitute;
using NUnit.Framework;
using Back_end.Services.Implementations;
using Back_end.Persistence.Interfaces;
using Back_end.Services.Interfaces;
using Org.BouncyCastle.Bcpg;
using Back_end.Objects;
using Back_end.Endpoints.Models;

namespace Tests;

[TestFixture]
public class UserServiceTest
{
    private UserService userService;
    private IUserPersistence userPersistenceMock;
    private IJobPersistence jobPersistenceMock;
    private IEmailService emailServiceMock;

    [SetUp]
    public void Setup()
    {
        userPersistenceMock = Substitute.For<IUserPersistence>();
        jobPersistenceMock = Substitute.For<IJobPersistence>();
        emailServiceMock = Substitute.For<IEmailService>();
        userService = new UserService(userPersistenceMock, jobPersistenceMock, emailServiceMock);
    }

    [Test]
    public void SaveJobValidInputs()
    {
        int userId = 1;
        int jobId = 5;
        var filters = CreateFilters("1", "5");

        userPersistenceMock.IsJobInLikes(userId, jobId).Returns(false);
        userPersistenceMock.SaveJob(userId, jobId).Returns(0);

        var result = userService.SaveJob(filters);
        Assert.That(result, Is.EqualTo(0));
        userPersistenceMock.Received(1).SaveJob(userId, jobId);
    }

    [Test]
    public void SaveAlreadyLikedJob()
    {
        var filters = CreateFilters("1", "5");

        userPersistenceMock.IsJobInLikes(1, 5).Returns(true);

        Assert.Throws<InvalidOperationException>(() => userService.SaveJob(filters));
    }

    [Test]
    public void SaveInvalidJob()
    {
        var filters = CreateFilters("1", "0");

        Assert.Throws<InvalidOperationException>(() => userService.SaveJob(filters));
    }

    [Test]
    public void SaveJobFromInvalidUser()
    {
        var filters = CreateFilters("0", "5");

        Assert.Throws<InvalidOperationException>(() => userService.SaveJob(filters));
    }

    [Test]
    public void SaveWithNullFilters()
    {
        Assert.Throws<InvalidOperationException>(() => userService.SaveJob(null));
    }

    [Test]
    public void UnsaveJobValidInputs()
    {
        int userId = 1;
        int jobId = 5;
        var filters = CreateFilters("1", "5");

        userPersistenceMock.IsJobInLikes(userId, jobId).Returns(true);
        userPersistenceMock.UnsaveJob(userId, jobId).Returns(true);

        var result = userService.UnsaveJob(filters);

        Assert.That(result, Is.True);
        userPersistenceMock.Received(1).UnsaveJob(userId, jobId);
    }

    [Test]
    public void UnsaveJobNotLikedYetThrowsException()
    {
        var filters = CreateFilters("1", "5");

        userPersistenceMock.IsJobInLikes(1, 5).Returns(false);

        var ex = Assert.Throws<InvalidOperationException>(() => userService.UnsaveJob(filters));
        Assert.That(ex.Message, Is.EqualTo("This job has not already been liked by this user"));
    }

    [Test]
    public void UnsaveJobNullFiltersThrowsException()
    {
        Assert.Throws<InvalidOperationException>(() => userService.UnsaveJob(null));
    }

    [Test]
    public void GetValidProfileById()
    {
        int userId = 1;
        var expectedUser = new User(userId, "test@test.com", "testuser", "1234", "", "test", "user", new List<Experience>());

        userPersistenceMock.GetUser(userId).Returns(expectedUser);
        var result = userService.GetProfile(userId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.UserId, Is.EqualTo(userId));
        Assert.That(result.Username, Is.EqualTo("testuser"));
    }

    [Test]
    public void GetProfileByInvalidUserIdReturnsException()
    {
        int userId = -1;
        userPersistenceMock.GetUser(userId).Returns((User?)null);

        Assert.Throws<NullReferenceException>(() => userService.GetProfile(userId));
    }

    [Test]
    public void GetValidProfileByUsername()
    {
        string username = "testuser";
        int userId = 1;
        var expectedUser = new User(userId, "test@test.com", username, "1234", "", "test", "user", new List<Experience>());

        userPersistenceMock.GetUserByUsername(username).Returns(expectedUser);
        var result = userService.GetProfileByUsername(username);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.UserId, Is.EqualTo(1));
        Assert.That(result.Username, Is.EqualTo("testuser"));
    }

    [Test]
    public void GetProfileByInvalidUsernameReturnsException()
    {
        string username = "rand";
        userPersistenceMock.GetUserByUsername(username).Returns((User?)null);

        Assert.Throws<NullReferenceException>(() => userService.GetProfileByUsername(username));
    }

    [Test]
    public void LoginWithValidCredentials()
    {
        string email = "test@test.com";
        string password = "1233";
        int userId = 1;
        LoginRequest loginRequest = new LoginRequest(email, password);
        var expectedUser = new User(userId, email, "testuser", password, "", "test", "user", new List<Experience>()) { Verified = true };

        userPersistenceMock.GetUserByCredentials(email.ToLower(), password).Returns(expectedUser);
        var result = userService.Login(loginRequest);

        Assert.That(result, Is.EqualTo(userId));
    }

    [Test]
    public void LoginWithBadCredentials()
    {
        string email = "test@test.com";
        string password = "1233";
        LoginRequest loginRequest = new LoginRequest(email, password);

        userPersistenceMock.GetUserByCredentials(email, password).Returns((User?)null);
        
        Assert.Throws<InvalidOperationException>(() => userService.Login(loginRequest));
    }


    private Dictionary<string, string> CreateFilters(string userId, string jobId)
    {
        return new Dictionary<string, string>
        {
            { "UserId", userId },
            { "JobId", jobId }
        };
    }
    
}