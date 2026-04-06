namespace Back_end.Services.Interfaces;

public interface IEmailService
{
    /// <summary>Sends an email verification link to the given address.</summary>
    /// <param name="toEmail">The recipient's email address.</param>
    /// <param name="token">The verification token to embed in the link.</param>
    public Task SendVerificationEmailAsync(string toEmail, string token);

    /// <summary>Sends a new job posting notification to all provided follower emails.</summary>
    /// <param name="posterName">The display name of the user who posted the job.</param>
    /// <param name="posterUsername">The username of the poster, used to build the profile link.</param>
    /// <param name="jobTitle">The title of the newly posted job.</param>
    /// <param name="followerEmails">List of email addresses to notify.</param>
    public Task SendJobNotificationEmailsAsync(string posterName, string posterUsername, string jobTitle, List<string> followerEmails);

    /// <summary>Notifies a user that someone commented on their profile.</summary>
    /// <param name="toEmail">The profile owner's email address.</param>
    /// <param name="posterUsername">The username of the person who left the comment.</param>
    /// <param name="profileUsername">The username of the profile that was commented on, used to build the profile link.</param>
    /// <param name="commentText">The content of the comment.</param>
    public Task SendProfileCommentNotificationAsync(string toEmail, string posterUsername, string profileUsername, string commentText);
}
