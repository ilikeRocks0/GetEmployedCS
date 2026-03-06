using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(folder_id))]
public class FolderEntity
{
    public int folder_id { get; set; }
    public int seeker_id { get; set; }
    public required string folder_name { get; set; }
}
