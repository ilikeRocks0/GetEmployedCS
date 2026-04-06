using Back_end.Objects;

public class QuizGameData
{
    public static List<QuizItem> OneQuiz = new List<QuizItem>
    {
        new QuizItem(
            weakSentence: "I created a RPG game over the summer",
            strongSentence: "Developed an RPG game in Unreal engine that has 25 concurrent players"
        )
    };

    public static List<QuizItem> QuizList = new List<QuizItem>
    {
        new QuizItem(
            weakSentence: "Helped with customer service at a retail store",
            strongSentence: "Resolved 40+ customer inquiries daily at a high-volume retail location, maintaining a 95% satisfaction rating"
        ),
        new QuizItem(
            weakSentence: "Sent out over 200 emails to clients every single week to let them know about updates and changes happening at the company",
            strongSentence: "Managed client communications through targeted email campaigns, maintaining a 48% open rate"
        ),
        new QuizItem(
            weakSentence: "Replied to more than 30% of customer complaints we received each day about various issues they were having with the product",
            strongSentence: "Resolved customer complaints with a 94% satisfaction score, flagging recurring issues that led to a product patch"
        ),
    };
}