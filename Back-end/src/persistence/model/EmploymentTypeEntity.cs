using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(employment_type_string))]
public class EmploymentTypeEntity
{
  public required string employment_type_string { get; set; }
}