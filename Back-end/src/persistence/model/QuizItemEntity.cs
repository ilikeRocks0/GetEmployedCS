using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Model;

[PrimaryKey(nameof(quiz_item_id))]
public class QuizItemEntity
{
    public int quiz_item_id { get; set; }
    public required string strong_sentence { get; set; }
    public required string weak_sentence { get; set; }
}
