//we dont want to expose which word is the strong one or weak one
public class QuizOptions
{
    public string sentence1 {get;set;}
    public string sentence2 {get;set;}

    public QuizOptions(QuizItem quizItem) 
    {
        Random rnd = new Random();
        if (rnd.Next(0, 2) == 0)
        {
            sentence1 = quizItem.strongSentence;
            sentence2 = quizItem.weakSentence;
        }
        else
        {
            sentence1 = quizItem.weakSentence;
            sentence2 = quizItem.strongSentence;
        }
    }
}