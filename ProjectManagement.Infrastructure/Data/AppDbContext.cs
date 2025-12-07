using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities.Users;
using ProjectManagement.Domain.Entities.Projects;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Entities.Collaborations;
using ProjectManagement.Domain.Entities.Tasks;

namespace ProjectManagement.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // 1️⃣ Add DbSets for your entities
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;


        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<ProjectMember> ProjectMembers { get; set; } = null!;
        public DbSet<ProjectGoal> ProjectGoal { get; set; } = null!;


        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Mention> Mentions { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;


        public DbSet<ProjectTask> ProjectTasks { get; set; } = null!;
        public DbSet<SubTask> SubTasks { get; set; } = null!;
        public DbSet<TaskAttachment> TaskAttachments { get; set; } = null!;


        // 2️⃣ Override OnModelCreating to configure relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite primary key for ProjectMember
            modelBuilder.Entity<ProjectMember>()
                .HasKey(pm => new { pm.ProjectId, pm.UserId });

            // Relationship: ProjectMember → Project
            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.Project)
                .WithMany(p => p.ProjectMembers)
                .HasForeignKey(pm => pm.ProjectId);

            // Relationship: ProjectMember → User
            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.User)
                .WithMany(u => u.ProjectMembers)
                .HasForeignKey(pm => pm.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)  // Add this collection in User entity
                .HasForeignKey(ur => ur.UserId);

            // Relationship to Role
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)  // Add this collection in Role entity
                .HasForeignKey(ur => ur.RoleId);
        }
    }
}
