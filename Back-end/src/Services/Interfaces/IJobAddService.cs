using Back_end.Endpoints.Models;
using Org.BouncyCastle.Bcpg;

namespace Back_end.Services.Interfaces;

public interface IJobAddService
{
    int AddNewJob(int UserId, NewJob newJob);
}