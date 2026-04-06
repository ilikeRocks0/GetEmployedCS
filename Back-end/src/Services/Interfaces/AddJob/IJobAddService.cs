using Back_end.Endpoints.Models;
using Org.BouncyCastle.Bcpg;

namespace Back_end.Services.Interfaces;

public interface IJobAddService
{
    /// Add a new job posted by a user. 
    /// <param name="UserId">An id corresponding to the user who is adding the new job.
    /// <param name="newJob">An object containing the attributes of the job to add, as provided from the front end.
    /// Returns the newly added job's id. 
    int AddNewJob(int UserId, NewJob newJob);
}