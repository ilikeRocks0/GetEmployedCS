namespace Back_end.Endpoints.Models;

public class QuizGameResponse
{
    public string answer { get; set; }
    
    public QuizGameResponse(string answer)
    {
        this.answer = answer;
    }
}