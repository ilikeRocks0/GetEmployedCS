using Back_end.Endpoints.Models;
using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Implementations;
using Back_end.Services.Interfaces;
using NSubstitute;

namespace test;

public class JobAddServiceTest
{
    private IUserPersistence userPersistence;
    private IJobPersistence jobPersistence;
    private IJobAddService jobAddService;
    
    [SetUp]
    public void Setup()
    {
        userPersistence = Substitute.For<IUserPersistence>();
        jobPersistence = Substitute.For<IJobPersistence>();
        jobAddService = new JobAddService(jobPersistence, userPersistence);
    }

    [Test]
    public void AddJobAsSeeker()
    {
        userPersistence.GetUser(JobAddServiceData.jobSeeker.UserId).Returns(JobAddServiceData.jobSeeker);
        Assert.DoesNotThrow(delegate{jobAddService.AddNewJob(JobAddServiceData.jobSeeker.UserId, JobAddServiceData.baseJob);});
    }

    [Test]
    public void AddJobAsEmployer()
    {
        userPersistence.GetUser(JobAddServiceData.employer.UserId).Returns(JobAddServiceData.employer);
        JobAddServiceData.baseJob.EmployerPoster = true;
        Assert.DoesNotThrow(delegate{jobAddService.AddNewJob(JobAddServiceData.employer.UserId, JobAddServiceData.baseJob);});
    }

    [Test]
    public void AddJobWithBadDateFormat()
    {
        userPersistence.GetUser(JobAddServiceData.jobSeeker.UserId).Returns(JobAddServiceData.jobSeeker);
        Assert.Throws<FormatException>(delegate{jobAddService.AddNewJob(JobAddServiceData.jobSeeker.UserId, JobAddServiceData.badDateFormatJob);});
    }

    [Test]
    public void AddJobWithBadLinkFormat()
    {
        userPersistence.GetUser(JobAddServiceData.jobSeeker.UserId).Returns(JobAddServiceData.jobSeeker);
        Assert.Throws<FormatException>(delegate{jobAddService.AddNewJob(JobAddServiceData.jobSeeker.UserId, JobAddServiceData.badLinkFormatJob);});
    }

    [Test]
    public void AddJobWithMultipleLocations()
    {
        userPersistence.GetUser(JobAddServiceData.jobSeeker.UserId).Returns(JobAddServiceData.jobSeeker);
        JobAddServiceData.baseJob.Locations = ["Winnipeg, MB, Canada", "Toronto, ON, Canada", "Washington, DC, USA"];
        Assert.DoesNotThrow(delegate{jobAddService.AddNewJob(JobAddServiceData.jobSeeker.UserId, JobAddServiceData.baseJob);});
    }

    [Test]
    public void AddJobWithMultipleLanguages()
    {
        userPersistence.GetUser(JobAddServiceData.jobSeeker.UserId).Returns(JobAddServiceData.jobSeeker);
        JobAddServiceData.baseJob.Locations = ["SQL", "C#", "Java", "Python"];
        Assert.DoesNotThrow(delegate{jobAddService.AddNewJob(JobAddServiceData.jobSeeker.UserId, JobAddServiceData.baseJob);});
    }
}