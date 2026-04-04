namespace test;

using Back_end.Persistence.Interfaces;
using Back_end.Objects;
using Back_end.Services.Implementations;
using Back_end.Services.Interfaces;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Back_end.Endpoints.Models;

public class CommentsServiceTest
{
    private ICommentsService commentsService;
    private IJobPersistence jobPersistence;
    private List<JobComment> comments;
    private NewJobComment newComment;
    private IUserPersistence userPersistence;
    private readonly User user = new(0, "email@gmail.com", "user", "pass", "I am cool guy!", "Test", "User", []);
    
    [SetUp]
    public void Setup()
    {
        jobPersistence = Substitute.For<IJobPersistence>();
        userPersistence = Substitute.For<IUserPersistence>();
        comments = [new JobComment("Epic comment", 1, 0, "thePoster")];
        newComment = new NewJobComment("Epic comment", user.UserId, 4);
        commentsService = new CommentsService(jobPersistence, userPersistence);
    }

    [Test]
    public void GetCommentNegativeJobId()
    {
        Assert.Throws<ArgumentException>(delegate{ commentsService.GetComments(-1); });
    }

    [Test]
    public void GetCommentsHappy()
    {
        jobPersistence.GetJobComments(0).Returns(comments);
        List<JobComment> result = commentsService.GetComments(0);
        Assert.That(result, Is.EqualTo(comments));
    }

    [Test]
    public void CreateCommentNullUser()
    {
        userPersistence.GetUser(newComment.PosterUserId).ReturnsNull();
        Assert.Throws<NullReferenceException>(delegate{ commentsService.CreateComment(newComment); });
    }

    [Test]
    public void CreateCommentNegativeJobId()
    {
        newComment.JobId = -2;
        userPersistence.GetUser(newComment.PosterUserId).Returns(user);
        Assert.Throws<ArgumentException>(delegate{ commentsService.CreateComment(newComment); });
    }

    [Test]
    public void CreateCommentEmpty()
    {
        newComment.Comment = "";
        userPersistence.GetUser(newComment.PosterUserId).Returns(user);
        Assert.Throws<ArgumentException>(delegate{ commentsService.CreateComment(newComment); });
    }

    [Test]
    public void CreateCommentSpace()
    {
        newComment.Comment = "     ";
        userPersistence.GetUser(newComment.PosterUserId).Returns(user);
        Assert.Throws<ArgumentException>(delegate{ commentsService.CreateComment(newComment); });
    }

    [Test]
    public void CreateCommentHappy()
    {
        userPersistence.GetUser(newComment.PosterUserId).Returns(user);
        JobComment jobComment = new JobComment(newComment.Comment, user.UserId, newComment.JobId, user.Username);
        jobPersistence.CreateJobComment(Arg.Any<JobComment>()).Returns(r => r.Arg<JobComment>()).AndDoes(jp => comments.Add(jobComment));
        
        JobComment result = commentsService.CreateComment(newComment);
        Assert.Multiple(() =>
        {
            Assert.That(result.Comment, Is.EqualTo(jobComment.Comment));
            Assert.That(result.PosterUserId, Is.EqualTo(jobComment.PosterUserId));
            Assert.That(result.PosterUsername, Is.EqualTo(jobComment.PosterUsername));
            Assert.That(result.JobId, Is.EqualTo(jobComment.JobId));
            Assert.That(comments, Does.Contain(jobComment));
        });
    }
}