using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(location_id))]
public class LocationEntity
{
    public int location_id { get; set; }
    public required string country { get; set; }
    public required string state { get; set; }
    public required string city { get; set; }
    public ICollection<JobLocationEntity>? jobs { get; set; }
}