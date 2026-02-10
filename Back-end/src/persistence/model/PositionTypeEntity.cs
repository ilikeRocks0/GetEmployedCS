using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(position_type_string))]
public class PositionTypeEntity
{
  public required string position_type_string { get; set; }
}