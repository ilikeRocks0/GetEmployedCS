using Back_end.Persistence.Implementations;
using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Services.Interfaces;
using Back_end.Util;
using Microsoft.AspNetCore.Identity;

namespace test;

public class JobServiceIntegrationTest : IntegrationTest
{
    private IUserPersistence userPersistence;
    private IJobPersistence jobPersistence;
    private IJobService jobService;
    private Job job1;
    private Job job2;
    private User user = new User(0, "email@email.com", "user", "pass", "about", "first", "last", []);

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        userPersistence = new UserPersistence(this.config, new PasswordHasher<User>());

        job1 = new Job("job1", null, user.FirstName + " " + user.LastName, false, "https://job.com", null, null, "Full stack", "Full-time", [], [], "description");
        job2 = new Job("job2", null, user.FirstName + " " + user.LastName, false, "https://job.com", null, null, "Full stack", "Full-time", [], [], "description");
        jobPersistence = new JobPersistence(this.config);
        
        jobService = new JobService(jobPersistence);
    }

    [Test]
    public void GetJobsIntegrationTest()
    {
        userPersistence.CreateUser(user);
        jobPersistence.CreateJob(job1);
        jobPersistence.CreateJob(job2);

        IReadOnlyDictionary<string, string> filters1 = new Dictionary<string, string>(){
            { AppConfig.FilterKeys.SEARCH_TERM, job1.JobTitle }, 
        };

        IReadOnlyList<Job> list1 = jobService.GetJobs(filters1);

        IReadOnlyDictionary<string, string> filters2 = new Dictionary<string, string>(){
            { AppConfig.FilterKeys.SEARCH_TERM, job2.JobTitle }, 
        };

        IReadOnlyList<Job> list2 = jobService.GetJobs(filters2);

        Assert.Multiple(() =>
        {
           Assert.That(list1, Has.Count.EqualTo(1));
           Assert.That(list1[0].JobTitle, Is.EqualTo(job1.JobTitle));
           Assert.That(list2, Has.Count.EqualTo(1));
           Assert.That(list2[0].JobTitle, Is.EqualTo(job2.JobTitle)); 
        });
    }

    [Test]
    public void GetSavedJobsIntegrationTest()
    {
        int userId = userPersistence.CreateUser(user);
        int jobId = jobPersistence.CreateJob(job1);
        jobPersistence.CreateJob(job2);

        IReadOnlyDictionary<string, string> filters = new Dictionary<string, string>(){
           { AppConfig.FilterKeys.USERID, userId.ToString() }, 
           { AppConfig.FilterKeys.JOBID, jobId.ToString() }
        };

        new UserService(userPersistence, jobPersistence).SaveJob(filters);

        IReadOnlyDictionary<string, string> jobFilters = new Dictionary<string, string>(){
            { AppConfig.FilterKeys.USERID, userId.ToString() },
            { AppConfig.FilterKeys.SEARCH_TERM, job1.JobTitle }, 
        };

        IReadOnlyList<Job> savedJobs = jobService.GetSavedJobs(jobFilters);

        Assert.Multiple(() =>
        {
            Assert.That(savedJobs, Has.Count.EqualTo(1));
            Assert.That(savedJobs[0].JobTitle, Is.EqualTo(job1.JobTitle));
        });
    }
}