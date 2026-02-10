using Microsoft.EntityFrameworkCore;

using Back_end.Persistence.Model;
using System.Data.Entity.Core;

namespace Back_end.Persistence.Implementations;

public class AppDbContext : DbContext
{
  private readonly string connectionString;
  public DbSet<EmployerEntity> Employers { get; set; }
  public DbSet<EmployerCommentEntity> EmployerComments { get; set; }
  public DbSet<ExperienceEntity> Experiences { get; set; }
  public DbSet<FolderEntity> Folders { get; set; }
  public DbSet<FolderContentEntity> FolderContents { get; set; }
  public DbSet<GenericWordEntity> GenericWords { get; set; }
  public DbSet<JobEntity> Jobs { get; set; }
  public DbSet<JobLocationEntity> JobLocations { get; set; }
  public DbSet<JobSeekerEntity> JobSeekers { get; set; }
  public DbSet<JobSeekerCommentEntity> JobSeekerComments { get; set; }
  public DbSet<LikeEntity> Likes { get; set; }
  public DbSet<LocationEntity> Locations { get; set; }
  public DbSet<QuizItemEntity> QuizItems { get; set; }
  public DbSet<UserEntity> Users { get; set; }

  public AppDbContext(IConfiguration config)
  {
    // Bind the values of the AppConfig section in appsettings.json to an AppConfig instance 
    AppConfig appConfig = new();
    config.GetSection(nameof(AppConfig)).Bind(appConfig);

    // Save connection string from environment variables, or throw exception if it returns null
    connectionString = Environment.GetEnvironmentVariable(appConfig.DBEnvConnectionString) ?? throw new ObjectNotFoundException();
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
      .HasKey(e => new {e.job_id, e.location_id});

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
      .HasKey(e => new {e.job_id, e.seeker_id});

    modelBuilder.Entity<LikeEntity>()
      .HasOne(e => e.jobSaver)
      .WithMany(e => e.likes)
      .HasForeignKey(e => e.seeker_id);

    modelBuilder.Entity<LikeEntity>()
      .HasOne(e => e.savedJob)
      .WithMany(e => e.likes)
      .HasForeignKey(e => e.job_id);
  }
}
