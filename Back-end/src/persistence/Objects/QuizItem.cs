using Back_end.Persistence.Objects;

public class QuizItem
{
    public int quizId {get; set;}
    public string weakSentence {get; set;}
    public string strongSentence {get; set;}

    public QuizItem(string weakSentence, string strongSentence)
    {
        this.weakSentence = weakSentence;
        this.strongSentence = strongSentence;
    }
}