using System.Data.Entity.Core;
using Back_end.Persistence.Model;
using Back_end.Util;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Persistence.Implementations;

public class AppDbContext : DbContext
{
    private readonly string connectionString;
    public DbSet<EmployerEntity> Employers { get; set; }
    public DbSet<ExperienceEntity> Experiences { get; set; }
    public DbSet<FolderEntity> Folders { get; set; }
    public DbSet<FolderContentEntity> FolderContents { get; set; }
    public DbSet<GenericWordEntity> GenericWords { get; set; }
    public DbSet<JobEntity> Jobs { get; set; }
    public DbSet<JobLanguageEntity> JobLanguages { get; set; }
    public DbSet<JobLocationEntity> JobLocations { get; set; }
    public DbSet<JobSeekerEntity> JobSeekers { get; set; }
    public DbSet<JobCommentEntity> JobComments { get; set; }
    public DbSet<LikeEntity> Likes { get; set; }
    public DbSet<LocationEntity> Locations { get; set; }
    public DbSet<ProgrammingLanguageEntity> ProgrammingLanguages { get; set; }
    public DbSet<QuizItemEntity> QuizItems { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserCommentEntity> UserComments { get; set; }

    public AppDbContext(IConfiguration config)
    {
        AppOptions options = new();
        config.GetSection(nameof(AppOptions)).Bind(options);

        // Save connection string from environment variables, or throw exception if it returns null
        this.connectionString = Environment.GetEnvironmentVariable(options.DBEnvConnectionString) ?? throw new ObjectNotFoundException();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(this.connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define relationship between Jobs and Users
        modelBuilder.Entity<JobEntity>()
          .HasOne(e => e.poster)
          .WithMany(e => e.postedJobs)
          .HasForeignKey(e => e.poster_id)
          .HasPrincipalKey(e => e.user_id);

        // Define relationship between Jobs, JobLocations, and Locations
        modelBuilder.Entity<JobLocationEntity>()
          .HasKey(e => new { e.job_id, e.location_id });

        modelBuilder.Entity<JobLocationEntity>()
          .HasOne(e => e.location)
          .WithMany(e => e.jobs)
          .HasForeignKey(e => e.location_id);

        modelBuilder.Entity<JobLocationEntity>()
          .HasOne(e => e.job)
          .WithMany(e => e.locations)
          .HasForeignKey(e => e.job_id);

        // Define relationship between JobSeekers, Likes, and Jobs
        modelBuilder.Entity<LikeEntity>()
          .HasKey(e => new { e.job_id, e.seeker_id });

        modelBuilder.Entity<LikeEntity>()
          .HasOne(e => e.jobSaver)
          .WithMany(e => e.likes)
          .HasForeignKey(e => e.seeker_id);

        modelBuilder.Entity<LikeEntity>()
          .HasOne(e => e.savedJob)
          .WithMany(e => e.likes)
          .HasForeignKey(e => e.job_id);

        // Define relationship between Jobs, JobProgrammingLanguages, and ProgrammingLanguages
        modelBuilder.Entity<JobLanguageEntity>()
          .HasKey(e => new { e.job_id, e.language_name });

        modelBuilder.Entity<JobLanguageEntity>()
          .HasOne(e => e.job)
          .WithMany(e => e.programmingLanguages)
          .HasForeignKey(e => e.job_id);

        modelBuilder.Entity<JobLanguageEntity>()
          .HasOne(e => e.language)
          .WithMany(e => e.jobs)
          .HasForeignKey(e => e.language_name);

        // Define relationship between Users and Employers/JobSeekers
        modelBuilder.Entity<UserEntity>()
          .HasOne(e => e.employer)
          .WithOne(e => e.user)
          .HasForeignKey<EmployerEntity>(e => e.user_id);

        modelBuilder.Entity<UserEntity>()
          .HasOne(e => e.jobSeeker)
          .WithOne(e => e.user)
          .HasForeignKey<JobSeekerEntity>(e => e.user_id);

        // Enforce unique usernames
        modelBuilder.Entity<UserEntity>()
          .HasIndex(e => e.username)
          .IsUnique();

        // Enforce unique emails
        modelBuilder.Entity<UserEntity>()
          .HasIndex(e => e.email)
          .IsUnique();

        // Define relationship between JobSeekers and Experiences
        modelBuilder.Entity<JobSeekerEntity>()
          .HasMany(e => e.experiences)
          .WithOne(e => e.jobSeeker)
          .HasForeignKey(e => e.seeker_id);

        // Define relationship between Users, JobComments, and Jobs
        modelBuilder.Entity<JobCommentEntity>()
          .HasOne(e => e.poster)
          .WithMany(e => e.jobComments)
          .HasForeignKey(e => e.poster_id);

        modelBuilder.Entity<JobCommentEntity>()
          .HasOne(e => e.job)
          .WithMany(e => e.comments)
          .HasForeignKey(e => e.job_id);
    }
}
