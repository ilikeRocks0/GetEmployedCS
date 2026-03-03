using Back_end.Persistence.Objects;

namespace Back_end.Persistence.Implementations.Adapters.EntityAdapters;

public class UserEntityAdapter : User
{
  public UserEntityAdapter(UserEntity userEntity) : base(userEntity.user_id, userEntity.email, userEntity.username, userEntity.password, userEntity.about_string)
  {
    if(userEntity.jobSeeker != null)
    {
      this.Experiences = new();
      
      // Convert the experience entities to logic objects
      userEntity.jobSeeker.experiences!.ToList().ForEach(e => this.Experiences.Add(new ExperienceEntityAdapter(e)));

      // Create new job seeker user object
      this.FirstName = userEntity.jobSeeker.first_name;
      this.LastName = userEntity.jobSeeker.last_name;
    }
    else
    {
      // Create new employer user object
      this.EmployerName = userEntity.employer!.employer_name;
    } 
  }  
}