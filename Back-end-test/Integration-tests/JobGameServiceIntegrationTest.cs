using System.Linq.Expressions;
using Back_end.Endpoints.Models;
using Back_end.Persistence.Implementations;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Implementations;
using Back_end.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace test;

public class JobGameServiceIntegrationTest : IntegrationTest
{
    private IUserPersistence userPersistence;
    private IJobPersistence jobPersistence;
    private IJobService jobService;
    private IJobGameService jobGameService;
    private User user;
    private Job job;
    
    [SetUp]
    public override void Setup()
    {
        base.Setup();

        user = new User(0, "email@email.com", "user", "pass", "about", "test", "user", []);
        userPersistence = new UserPersistence(this.config, new PasswordHasher<User>());
        user.UserId = userPersistence.CreateUser(user);

        job = new Job("job", null, user.FirstName + " " + user.LastName, false, "https://job.com", null, null, "Full stack", "Full-time", [], [], "description");
        jobPersistence = new JobPersistence(this.config);
        job.JobId = jobPersistence.CreateJob(job);

        jobService = new JobService(jobPersistence);
        jobGameService = new JobGameService(userPersistence, jobPersistence, jobService);
    }

    [Test]
    public void RejectJobIntegrationTest()
    {
        CurrentUser currentUser = new CurrentUser(user.UserId);
        Job? next = jobGameService.InitializeJobGame(currentUser);

        Assert.That(next is not null);

        GameJob gameJob = new(user.UserId, next!.JobId);

        next = jobGameService.RejectJob(gameJob);
        (int accepted, int rejected) = jobGameService.GetGameStats(currentUser);

        Assert.Multiple(() =>
        {
            Assert.That(next is null);
            Assert.That(accepted, Is.EqualTo(0));
            Assert.That(rejected, Is.EqualTo(1));
        });
    }

    [Test]
    public void AcceptJobIntegrationTest()
    {
        CurrentUser currentUser = new CurrentUser(user.UserId);
        Job? next = jobGameService.InitializeJobGame(currentUser);

        Assert.That(next is not null);

        GameJob gameJob = new(user.UserId, next!.JobId);

        next = jobGameService.AcceptJob(gameJob);
        (int accepted, int rejected) = jobGameService.GetGameStats(currentUser);

        Assert.Multiple(() =>
        {
            Assert.That(next is null);
            Assert.That(accepted, Is.EqualTo(1));
            Assert.That(rejected, Is.EqualTo(0));
        });
    }
}