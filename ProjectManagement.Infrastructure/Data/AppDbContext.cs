using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities.Users;
using ProjectManagement.Domain.Entities.Projects;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Entities.Collaborations;
using ProjectManagement.Domain.Entities.ProjectTasks;

namespace ProjectManagement.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // 1️⃣ Add DbSets for your entities
        public DbSet<User> User { get; set; } = null!;
        public DbSet<Role> Role { get; set; } = null!;
        //public DbSet<UserRole> UserRole { get; set; } = null!;


        public DbSet<Project> Project { get; set; } = null!;
        public DbSet<ProjectMember> ProjectMember { get; set; } = null!;
        public DbSet<ProjectGoal> ProjectGoals { get; set; } = null!;


        public DbSet<Comment> Comment { get; set; } = null!;
        public DbSet<Mention> Mention { get; set; } = null!;
        public DbSet<Notification> Notification { get; set; } = null!;


        public DbSet<ProjectTask> ProjectTask { get; set; } = null!;
        public DbSet<SubTask> SubTask { get; set; } = null!;
        public DbSet<TaskAttachment> TaskAttachments { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");

            // restrict the delete, when the project being deleted, the leader won't be deleted
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Leader)
                .WithMany()
                .HasForeignKey(p => p.LeaderId)
                .OnDelete(DeleteBehavior.Restrict);


            // -------------------------------
            // ProjectMember composite key
            // -------------------------------
            modelBuilder.Entity<ProjectMember>()
                .HasKey(pm => new { pm.ProjectId, pm.UserId });

            // ProjectMember → Project
            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.Project)
                .WithMany(p => p.ProjectMembers)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProjectMember → User
            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.User)
                .WithMany(u => u.ProjectMembers)
                .HasForeignKey(pm => pm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------------------
            // UserRole composite key
            // -------------------------------
            ///modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });

            // UserRole → User
            // modelBuilder.Entity<UserRole>()
            //     .HasOne(ur => ur.User)
            //     .WithMany(u => u.UserRoles)
            //     .HasForeignKey(ur => ur.UserId)
            //     .OnDelete(DeleteBehavior.Restrict);

            // // UserRole → Role
            // modelBuilder.Entity<UserRole>()
            //     .HasOne(ur => ur.Role)
            //     .WithMany(r => r.UserRoles)
            //     .HasForeignKey(ur => ur.RoleId)
            //     .OnDelete(DeleteBehavior.Restrict);

            // -------------------------------
            // ProjectTask key
            // -------------------------------
            // modelBuilder.Entity<ProjectTask>()
            //     .HasKey(t => t.Id);

            // modelBuilder.Entity<ProjectTask>()
            //     .HasKey(t => t.Id);
            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.AssignToUser)
                .WithMany(u => u.ProjectTasks) 
                .HasForeignKey(t => t.AssignToUserId)
                .OnDelete(DeleteBehavior.Restrict); // 改成 Restrict 或 NoAction

            // -------------------------------
            // Mentions
            // -------------------------------
            modelBuilder.Entity<Mention>()
                .HasOne(m => m.MentionedUser)
                .WithMany(u => u.Mentions) // navigation collection in User
                .HasForeignKey(m => m.MentionedUserId)
                .OnDelete(DeleteBehavior.Restrict); // prevent multiple cascade paths

            modelBuilder.Entity<Mention>()
                .HasOne(m => m.Comment)
                .WithMany(c => c.Mentions) // navigation collection in Comment
                .HasForeignKey(m => m.CommentId)
                .OnDelete(DeleteBehavior.Cascade); // safe, single path

            // -------------------------------
            // Notifications
            // -------------------------------
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications) // add collection in User
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict); // prevent multiple cascade paths

            // Optional: Notification → Task/Comment/Project (nullable FKs)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.ProjectTask)
                .WithMany(t => t.Notifications)
                .HasForeignKey(n => n.ProjectTaskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Comment)
                .WithMany(c => c.Notifications)
                .HasForeignKey(n => n.CommentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Project)
                .WithMany(p => p.Notifications)
                .HasForeignKey(n => n.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------------------
            // Ignore system types
            // -------------------------------
            //modelBuilder.Ignore<System.Threading.Tasks.Task>();
        }
    }
}