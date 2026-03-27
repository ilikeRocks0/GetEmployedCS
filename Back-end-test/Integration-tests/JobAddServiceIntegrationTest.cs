using Back_end.Endpoints.Models;
using Back_end.Persistence.Implementations;
using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace test;

public class JobAddServiceIntegrationTest : IntegrationTest
{
    private IJobPersistence jobPersistence;
    private IUserPersistence userPersistence;
    private IJobAddService jobAddService;
    private User user = new User(0, "email@email.com", "user", "pass", "about", "first", "last", []);

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        jobPersistence = new JobPersistence(this.config);
        userPersistence = new UserPersistence(this.config, new PasswordHasher<User>());
        user.UserId = userPersistence.CreateUser(user);

        jobAddService = new JobAddService(jobPersistence, userPersistence);
    }

    [Test]
    public void AddNewJobIntegrationTest()
    {
        NewJob newJob = new NewJob("title", "", "https://job.com", false, false, "Full stack", "Full-time", [], [], "description", false);
        int jobId = jobAddService.AddNewJob(user.UserId, newJob);

        Job? job = jobPersistence.GetJobFromJobId(jobId);
        

        Assert.Multiple(() =>
        {
            Assert.That(job is not null);
            Assert.That(job!.JobTitle, Is.EqualTo(newJob.Title));
            Assert.That(job!.JobDescription, Is.EqualTo(newJob.JobDescription));
            Assert.That(job!.ApplicationLink, Is.EqualTo(newJob.ApplicationLink));
            Assert.That(job!.EmployerPoster, Is.EqualTo(newJob.EmployerPoster));
            Assert.That(job!.PositionType, Is.EqualTo(newJob.PositionType));
            Assert.That(job!.EmploymentType, Is.EqualTo(newJob.EmploymentType));
        });
    }
}