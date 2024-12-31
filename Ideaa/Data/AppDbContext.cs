using GraduationProjectIdeasPlatform.Data.Models;
using Ideaa.Comment;
using Ideaa.Data.Models;
using Ideas.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ideas.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<IdeaSupervisor> IdeaSupervisors { get; set; }
        public DbSet<IdeaStudent> IdeaStudents { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new Comment_Relationships().ConfigureIdeaSupervisorRelationshipKey();
            modelBuilder.Entity<IdeaSupervisor>()
                .HasKey(x => new { x.IdeaId, x.UserId });

            new Comment_Relationships().ConfigureIdeaStudentRelationshipKey();
            modelBuilder.Entity<IdeaStudent>()
                .HasKey(x => new { x.IdeaId, x.UserId });

            new Comment_Relationships().ConfigureNotificationIdeaRelationship();
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            new Comment_Relationships().ConfigureNotificationIdeaRelationship();
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Idea)
                .WithMany(i => i.Notifications)
                .HasForeignKey(n => n.IdeaId)
                .OnDelete(DeleteBehavior.Cascade);

            new Comment_Relationships().ConfigureIdeaSupervisorInverseRelationship();
            modelBuilder.Entity<IdeaSupervisor>()
                .HasOne(n => n.Idea)
                .WithMany(i => i.IdeaSupervisors)
                .HasForeignKey(n => n.IdeaId);

            new Comment_Relationships().ConfigureIdeaSupervisorToUserRelationship();
            modelBuilder.Entity<IdeaSupervisor>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId);

            new Comment_Relationships().ConfigureIdeaStudentInverseRelationship();
            modelBuilder.Entity<IdeaStudent>()
                .HasOne(n => n.Idea)
                .WithMany(i => i.IdeaStudents)
                .HasForeignKey(n => n.IdeaId);

            new Comment_Relationships().ConfigureIdeaStudentToUserRelationship();
            modelBuilder.Entity<IdeaStudent>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId);
        }
    }
}
