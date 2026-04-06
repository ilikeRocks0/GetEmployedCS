using Back_end.Endpoints.Models;
using Back_end.Persistence.Implementations;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Interfaces;
using Back_end.Util;
using Microsoft.AspNetCore.Identity;
using Back_end.Services.Implementations;
using NSubstitute;

namespace test;

public class UserServiceIntegrationTest : IntegrationTest
{
    private IUserPersistence userPersistence;
    private IUserService userService;
    private IEmailService emailServiceMock;
    private NewUser newUser;
    private User user;
    private Job job;

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        user = new User(0, "email@email.com", "user", "pass", "about", "test", "user", []);
        newUser = new NewUser("user", "pass", "email@email.com", false, "test", "user", "");
        job = new Job("job", null, user.FirstName + " " + user.LastName, false, "https://job.com", null, null, "Full stack", "Full-time", [], [], "description");
        userPersistence = new UserPersistence(this.config, new PasswordHasher<User>());
        emailServiceMock = Substitute.For<IEmailService>();
        userService = new UserService(userPersistence, new JobPersistence(this.config), emailServiceMock);
    }

    [Test]
    public async Task CreateUserIntegrationTest()
    {
        int userId = await userService.CreateUser(newUser);

        User? user = userPersistence.GetUser(userId);
        Assert.Multiple(() =>
        {
           Assert.That(user is not null);
           Assert.That(user!.FirstName, Is.EqualTo(newUser.FirstName));
           Assert.That(user!.LastName, Is.EqualTo(newUser.LastName));
           Assert.That(user!.Email, Is.EqualTo(newUser.Email));
        });
    }

    [Test]
    public void SaveJobIntegrationTest()
    {
        int userId = userPersistence.CreateUser(user).userId;
        IJobPersistence jobPersistence = new JobPersistence(this.config);
        int jobId = jobPersistence.CreateJob(job);
        job.JobId = jobId;

        IReadOnlyDictionary<string, string> filters = new Dictionary<string, string>(){
           { AppConfig.FilterKeys.USERID, userId.ToString() },
           { AppConfig.FilterKeys.JOBID, jobId.ToString() }
        };

        userService.SaveJob(filters);

        List<Job> savedJobs = jobPersistence.GetSavedJobs(userId, "", [], [], [], 0, 100);

        Assert.Multiple(() =>
        {
            Assert.That(savedJobs, Has.Count.EqualTo(1));
            Assert.That(savedJobs[0].JobTitle, Is.EqualTo(job.JobTitle));
            Assert.That(savedJobs[0].EmployerPoster, Is.EqualTo(job.EmployerPoster));
            Assert.That(savedJobs[0].ApplicationLink, Is.EqualTo(job.ApplicationLink));
            Assert.That(savedJobs[0].PositionType, Is.EqualTo(job.PositionType));
            Assert.That(savedJobs[0].EmploymentType, Is.EqualTo(job.EmploymentType));
            Assert.That(savedJobs[0].JobDescription, Is.EqualTo(job.JobDescription));
        });
    }

    [Test]
    public void LoginIntegrationTest()
    {
        var (userId, token) = userPersistence.CreateUser(user);
        userPersistence.VerifyUser(token);

        int loginId = userService.Login(new LoginRequest(user.Email, user.Password));

        Assert.That(userId, Is.EqualTo(loginId));
    }

    [Test]
    public void UpdateUserIntegrationTest()
    {
        int userId = userPersistence.CreateUser(user).userId;

        UpdateUserRequest updateUser = new UpdateUserRequest()
        {
            Username = "updatedUser",
            FirstName = "updatedFirst",
            LastName = "updatedLast"
        };

        Profile profile = userService.UpdateUser(updateUser, userId);

        Assert.Multiple(() =>
        {
            Assert.That(profile.Username, Is.EqualTo(updateUser.Username));
            Assert.That(profile.FirstName, Is.EqualTo(updateUser.FirstName));
            Assert.That(profile.LastName, Is.EqualTo(updateUser.LastName));
        });
    }
}
