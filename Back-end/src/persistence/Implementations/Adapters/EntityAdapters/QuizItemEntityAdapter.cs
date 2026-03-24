using Back_end.Persistence.Model;

public class QuizItemEntityAdapter : QuizItem
{
    private void ValidateEntity(QuizItemEntity quizItemEntity)
    {
        if (quizItemEntity.strong_sentence.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("QuizItemEntity entity cannot have empty strong sentence.");
        }

        if (quizItemEntity.weak_sentence.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("QuizItemEntity entity cannot have empty weak setence.");
        }
    }

    public QuizItemEntityAdapter(QuizItemEntity quizItemEntity) : base(quizItemEntity.weak_sentence, quizItemEntity.strong_sentence)
    {
        ValidateEntity(quizItemEntity);
    }
}