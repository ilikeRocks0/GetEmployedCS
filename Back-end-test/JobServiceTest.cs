namespace Tests;

using Back_end.Persistence.Interfaces;
using Back_end.Services.Interfaces;
using NSubstitute;
using NUnit.Framework;

public class JobServiceTest()
{
    private IJobService jobService;
    private IJobPersistence jobPersistenceMock;

    [SetUp]
    public void Setup()
    {
        jobPersistenceMock = Substitute.For<IJobPersistence>();        
        jobService = new JobService(jobPersistenceMock);
    }

    //GetJobs tests:
    //just get all jobs 
    //get all jobs with a search term filter
    //get all jobs with a language filter
    //get all jobs with a position filter
    //get all jobs with employments filter
    //get all jobs with multiple filters (but not all) applied
    //get all jobs with all filters applied - should get a job
    //get all jobs with all filters applied - should get no matches 

    //GetJobsSavedSublist tests:
    //just get all saved jobs in jobs list
    //get all saved jobs in jobs listwith a search term filter
    //get all saved jobs in jobs list with a language filter
    //get all saved jobs in jobs list with a position filter
    //get all saved jobs in jobs listwith employments filter
    //get all saved jobs in jobs list with multiple filters (but not all) applied
    //get all saved jobs in jobs list with all filters applied - should get a job
    //get all saved jobs in jobs list with all filters applied - should get no matches

    //GetSavedJobs tests:
    //just get all saved jobs 
    //get all saved jobs with a search term filter
    //get all saved jobs with a language filter
    //get all saved jobs with a position filter
    //get all saved jobs with employments filter
    //get all saved jobs with multiple filters (but not all) applied
    //get all saved jobs with all filters applied - should get a job
    //get all saved jobs with all filters applied - should get no matches 

    //GetNumberOfJobs tests:
    //just get all jobs total amount  
    //get amount with a search term filter
    //get amount with a language filter
    //get amount with a position filter
    //get amount with employments filter
    //get amount with multiple filters (but not all) applied
    //get amount with all filters applied - should get a job
    //get amount with all filters applied - should get no matches 

    //GetNumberOfSavedJobs tests:
    //just get all saved jobs total amount  
    //get amount with a search term filter
    //get amount with a language filter
    //get amount with a position filter
    //get amount with employments filter
    //get amount with multiple filters (but not all) applied
    //get amount with all filters applied - should get a job
    //get amount with all filters applied - should get no matches 
}