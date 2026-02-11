namespace Back_end.Persistence.Implementations.Types;

public class ApplicationDate
{
  public DateOnly? Date { get; } = null;

  public ApplicationDate(DateTime? dateTime)
  {
    if(dateTime != null)
    {
      this.Date = DateOnly.FromDateTime((DateTime)dateTime);
    }
  }
}