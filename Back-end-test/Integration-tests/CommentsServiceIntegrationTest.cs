namespace test;

using Back_end.Persistence.Implementations;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Implementations;
using Back_end.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using Back_end.Endpoints.Models;

public class CommentsServiceIntegrationTest : IntegrationTest
{
    private ICommentsService commentsService;
    private IJobPersistence jobPersistence;
    private IUserPersistence userPersistence;


    [SetUp]
    public override void Setup()
    {
        base.Setup();
        jobPersistence = new JobPersistence(this.config);
        userPersistence = new UserPersistence(this.config, new PasswordHasher<User>());
        commentsService = new CommentsService(jobPersistence, userPersistence);
    }

    [Test]
    public void CreateCommentIntegrationTest()
    {
        User user = new User(0, "email@gmail.com", "newuser", "pass", "about", "Test", "User", []);
        user.UserId = userPersistence.CreateUser(user);

        Job job = new Job("Title", null, user.FirstName + " " + user.LastName, false, "https://google.com", null, null, "Full stack", "Co-op", [], [], "description");
        job.JobId = jobPersistence.CreateJob(job);

        Assert.DoesNotThrow(delegate {commentsService.CreateComment(new NewJobComment("Nice job", user.UserId, job.JobId)); });
    }
}