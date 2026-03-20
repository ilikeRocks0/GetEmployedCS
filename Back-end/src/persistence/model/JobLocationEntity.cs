using System.Diagnostics.CodeAnalysis;
using Back_end.Persistence.Model;
using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(job_id), nameof(location_id))]
public class JobLocationEntity
{
    public int job_id { get; set; }
    public int location_id { get; set; }
    public JobEntity? job { get; set; }
    public LocationEntity? location { get; set; }

    public JobLocationEntity(int job_id, int location_id)
    {
        this.job_id = job_id;
        this.location_id = location_id;
    }
}