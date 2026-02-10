using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(employer_id))]
public class EmployerEntity
{
    public int employer_id { get; set; }
    public int user_id{ get; set; }
    public required string employer_name { get; set; }
}
