using Back_end.Services.Interfaces;
using Resend;

namespace Back_end.Services.Implementations;

public class EmailService(IResend resend, IConfiguration config) : IEmailService
{
    private readonly string fromAddress = config["EMAIL_FROM"]
        ?? throw new InvalidOperationException("EMAIL_FROM environment variable is required");

    private readonly string appUrl = config["APP_URL"]
        ?? throw new InvalidOperationException("APP_URL environment variable is required");

    public async Task SendVerificationEmailAsync(string toEmail, string token)
    {
        var verificationLink = $"{appUrl}/verify?token={token}";

        var message = new EmailMessage
        {
            From = fromAddress
        };
        message.To.Add(toEmail);
        message.Subject = "Verify your GetEmployed account";
        message.HtmlBody = $"""
            <div style="font-family: sans-serif; max-width: 480px; margin: 0 auto;">
                <h2>Verify your email</h2>
                <p>Click the button below to verify your account. This link is valid for 24 hours.</p>
                <a href="{verificationLink}"
                   style="display: inline-block; padding: 12px 24px; background-color: #1677ff;
                          color: white; text-decoration: none; border-radius: 6px; font-weight: 600;">
                    Verify Email
                </a>
                <p style="margin-top: 16px; color: #888; font-size: 13px;">
                    If you did not create an account, you can ignore this email.
                </p>
            </div>
            """;

        await resend.EmailSendAsync(message);
    }

    public async Task SendJobNotificationEmailsAsync(string posterName, string posterUsername, string jobTitle, List<string> followerEmails)
    {
        var profileLink = $"{appUrl}/profile/{posterUsername}";

        var tasks = followerEmails.Select(email =>
        {
            var message = new EmailMessage { From = fromAddress };
            message.To.Add(email);
            message.Subject = $"New job posting from {posterName}";
            message.HtmlBody = $"""
                <div style="font-family: sans-serif; max-width: 480px; margin: 0 auto;">
                    <h2>New job posted by {posterName}</h2>
                    <p><strong>Someone you follow posted a job! {jobTitle}</strong> is now available on {posterName}'s profile.</p>
                    <a href="{profileLink}"
                       style="display: inline-block; padding: 12px 24px; background-color: #1677ff;
                              color: white; text-decoration: none; border-radius: 6px; font-weight: 600;">
                        View Profile
                    </a>
                </div>
                """;
            return resend.EmailSendAsync(message);
        });

        await Task.WhenAll(tasks);
    }

    public async Task SendProfileCommentNotificationAsync(string toEmail, string posterUsername, string profileUsername, string commentText)
    {
        var profileLink = $"{appUrl}/profile/{profileUsername}";

        var message = new EmailMessage { From = fromAddress };
        message.To.Add(toEmail);
        message.Subject = $"{posterUsername} commented on your profile";
        message.HtmlBody = $"""
            <div style="font-family: sans-serif; max-width: 480px; margin: 0 auto;">
                <h2>New comment on your profile</h2>
                <p><strong>{posterUsername}</strong> said:</p>
                <blockquote style="border-left: 3px solid #1677ff; margin: 0; padding: 8px 16px; color: #555;">
                    {commentText}
                </blockquote>
                <a href="{profileLink}"
                   style="display: inline-block; margin-top: 16px; padding: 12px 24px; background-color: #1677ff;
                          color: white; text-decoration: none; border-radius: 6px; font-weight: 600;">
                    View Profile
                </a>
            </div>
            """;

        await resend.EmailSendAsync(message);
    }
}
