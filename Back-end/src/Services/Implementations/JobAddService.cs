using Back_end.Endpoints.Models;
using Back_end.Persistence.Implementations.Validation;
using Back_end.Persistence.Interfaces;
using Back_end.Persistence.Objects;
using Back_end.Services.Implementations.Finders;
using Back_end.Services.Interfaces;

public class JobAddService(IJobPersistence jobPersistence, IUserPersistence userPersistence) : IJobAddService
{
    public int AddNewJob(int userId, NewJob newJob)
    {
        Job extractedJob = ExtractJobFromInput(userId, newJob);
        return jobPersistence.CreateJob(extractedJob);
    }

    private Job ExtractJobFromInput(int userId, NewJob NewJob)
    {
        Job extractedJob;

        //extracting the appropriate poster name depending if they're a company or job seeker
        UserFinder userFinder = new UserFinder(userPersistence);
        User? user = userFinder.GetUser(userId);
        if (user == null)
        {
            throw new NullReferenceException("User not found.");   
        }

        string posterName = ""; 
        if (NewJob.EmployerPoster && user.EmployerName != null)
        {
            posterName = user.EmployerName;
        }
        else if (user.FirstName != null && user.LastName != null)
        {
            posterName = user.FirstName + " " + user.LastName;
        }

        //setup for the rest of the Job attributes
        DateOnly? deadlineDate = null;
        if (!NewJob.Deadline.Equals(String.Empty))
        {
            bool isValidDate = DateOnly.TryParse(NewJob.Deadline, out DateOnly parsedDate);
            if (!isValidDate)
            {
                throw new FormatException("Invalid date format. Only provide dates in formats like YYYY-MM-DD");
            }
            else
            {
                deadlineDate = parsedDate;
            }
        }    
        
        bool isValidLink = ValidationRegex.linkRegex.IsMatch(NewJob.ApplicationLink);
        if (!isValidLink)
        {
            throw new FormatException("Invalid application link format.");
        }

        //if all attributes in NewJob (or converted/verified counterparts) are all good, make the new Job
        extractedJob = new Job(
            NewJob.Title,
            deadlineDate,
            posterName, 
            NewJob.EmployerPoster,
            NewJob.ApplicationLink, 
            NewJob.HasRemote, 
            NewJob.HasHybrid,             
            NewJob.PositionType,
            NewJob.EmploymentType,
            NewJob.Locations, 
            NewJob.ProgrammingLanguages,
            NewJob.JobDescription
        );
        
        return extractedJob;
    }
}