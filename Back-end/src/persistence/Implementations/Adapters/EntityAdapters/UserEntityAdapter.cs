using Back_end.Persistence.Implementations.Validation;
using Back_end.Persistence.Model;
using Back_end.Objects;
using Back_end.Persistence.Exceptions;

namespace Back_end.Persistence.Implementations.Adapters.EntityAdapters;

public class UserEntityAdapter : User
{
    private static void ValidateEntity(UserEntity userEntity)
    {
        if (userEntity.user_id < 0)
        {
            throw new ObjectConversionException("User entity cannot have negative user ID.");
        }

        if (userEntity.username.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("User entity cannot have empty username.");
        }

        if (!ValidationRegex.IsValidEmail(userEntity.email))
        {
            throw new ObjectConversionException(
                $"User entity must have a valid email (user_id={userEntity.user_id}).");
        }

        if (userEntity.password.Trim().Equals(String.Empty))
        {
            throw new ObjectConversionException("User entity cannot have empty password.");
        }

        if (userEntity.jobSeeker is null && userEntity.employer is null)
        {
            throw new ObjectConversionException("User entity is not related to any job seeker or employer entity.");
        }
    }

    public UserEntityAdapter(UserEntity userEntity) : base(userEntity.user_id, userEntity.email, userEntity.username, userEntity.password, userEntity.about_string)
    {
        ValidateEntity(userEntity);
        this.Verified = userEntity.verified;

        if (userEntity.jobSeeker != null)
        {
            this.Experiences = new();

            // Convert the experience entities to logic objects
            userEntity.jobSeeker.experiences?.ToList().ForEach(e => this.Experiences.Add(new ExperienceEntityAdapter(e)));

            // Create new job seeker user object
            this.FirstName = userEntity.jobSeeker.first_name;
            this.LastName = userEntity.jobSeeker.last_name;
            this.IsEmployer = false;
        }
        else if (userEntity.employer != null)
        {
            // Create new employer user object
            this.EmployerName = userEntity.employer.employer_name;
            this.IsEmployer = true;
        }
    }
}