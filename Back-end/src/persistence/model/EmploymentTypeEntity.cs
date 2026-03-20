using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(employment_type))]
public class EmploymentTypeEntity
{
    public required string employment_type;
}