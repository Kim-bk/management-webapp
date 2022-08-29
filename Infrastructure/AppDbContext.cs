using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain.AggregateModels.TaskAggregate;
using Domain.AggregateModels.ProjectAggregate;
using Domain.AggregateModels.UserAggregate;

namespace Infrastructure.Context
{
    public partial class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Label> Labels { get; set; }
        public virtual DbSet<ListTask> ListTasks { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<Todo> Todos { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

      /*  protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");


            modelBuilder.Entity<History>(entity =>
            {
                entity.ToTable("History");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Label>(entity =>
            {
                entity.ToTable("Label");
            });

            modelBuilder.Entity<ListTask>(entity =>
            {
                entity.ToTable("ListTask");

                entity.HasIndex(e => e.ProjectId, "IX_ListTask_ProjectId");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ListTasks)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK__ListTask__Projec__123EB7A3");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Task");

                entity.HasIndex(e => e.ListTaskId, "IX_Task_ListTaskId");

                entity.HasIndex(e => e.DoingId, "IX_Task_UserId");

                entity.HasOne(d => d.ListTask)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.ListTaskId)
                    .HasConstraintName("FK__Task__ListTaskId__151B244E");
            });

            modelBuilder.Entity<Todo>(entity =>
            {
                entity.ToTable("Todo");

                entity.HasIndex(e => e.TaskId, "IX_Todo_TaskId");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.Todos)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK__Todo__TaskId__18EBB532");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);*/
    }
}
