using Back_end.Persistence.Model;
using Back_end.Objects;
using Back_end.Persistence.Exceptions;

namespace Back_end.Persistence.Implementations.Adapters.EntityAdapters;

public class UserCommentEntityAdapter : UserComment
{
    public UserCommentEntityAdapter(UserCommentEntity userCommentEntity) : base(userCommentEntity.comment, userCommentEntity.poster_id, userCommentEntity.profile_id, userCommentEntity.posterUser!.username)
    {
        ValidateEntity(userCommentEntity);
    }

    private static void ValidateEntity(UserCommentEntity userCommentEntity)
    {
        if(userCommentEntity.comment.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("User comment entity must have non-empty comment");
        }

        if(userCommentEntity.poster_id < 0)
        {
            throw new ObjectConversionException("User comment entity must have non-negative poster user ID");
        }

        if(userCommentEntity.profile_id < 0)
        {
            throw new ObjectConversionException("User comment entity must have non-negative profile user ID");
        }

        if(userCommentEntity.posterUser!.username.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("User comment entity must have non-empty username");
        }
    }
}