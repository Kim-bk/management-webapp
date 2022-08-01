﻿using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain.Entities;

namespace API.Context
{
    public partial class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Label> Labels { get; set; }
        public virtual DbSet<LabelTask> LabelTasks { get; set; }
        public virtual DbSet<ListTask> ListTasks { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<Todo> Todos { get; set; }
        public virtual DbSet<TaskMember> TaskMember { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS14;Database=ManagementWebapp;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
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

            modelBuilder.Entity<LabelTask>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("LabelTask");

                entity.HasIndex(e => e.LabelId, "IX_LabelTask_LabelId");

                entity.HasIndex(e => e.TaskId, "IX_LabelTask_TaskId");

                entity.HasOne(d => d.Label)
                    .WithMany()
                    .HasForeignKey(d => d.LabelId)
                    .HasConstraintName("FK__LabelTask__Label__1DB06A4F");

                entity.HasOne(d => d.Task)
                    .WithMany()
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK__LabelTask__TaskI__32E0915F");
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

                entity.HasIndex(e => e.UserId, "IX_Task_UserId");

                entity.HasOne(d => d.ListTask)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.ListTaskId)
                    .HasConstraintName("FK__Task__ListTaskId__151B244E");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Task__UserId__160F4887");
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
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
