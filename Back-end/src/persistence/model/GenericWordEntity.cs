using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(generic_word_id))]
public class GenericWordEntity
{
    public int generic_word_id { get; set; }
    public required string word { get; set; }
}
