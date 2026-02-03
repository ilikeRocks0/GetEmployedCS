using Back_end.Persistence.Model;

namespace Back_end.Persistence.Implementations.Types;

public class JobLocation
{
  public string Location { get; }

  public JobLocation(LocationEntity locationEntity)
  {
    this.Location = locationEntity.city + ", " + locationEntity.state + ", " + locationEntity.country;
  }
}