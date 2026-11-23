﻿using Microsoft.EntityFrameworkCore;
using Al_Campus_Assistant.Models;

namespace Al_Campus_Assistant.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // جميع الـDbSets
    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Professor> Professors { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Filter> Filters { get; set; }
    public DbSet<Lecture> Lectures { get; set; }
    public DbSet<LectureMaterial> LectureMaterials { get; set; }
    public DbSet<LectureAttendance> LectureAttendances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
        });
        
        // Filter - User relationship
        modelBuilder.Entity<Filter>()
            .HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Professor - Course relationship
        modelBuilder.Entity<Professor>()
            .HasMany(p => p.Courses)
            .WithOne(c => c.Professor)
            .HasForeignKey(c => c.ProfessorId);
        
        // Course - Exam relationship
        modelBuilder.Entity<Course>()
            .HasMany(c => c.Exams)
            .WithOne(e => e.Course)
            .HasForeignKey(e => e.CourseId);
        
        // Filter configuration
        modelBuilder.Entity<Filter>(entity =>
        {
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.FilterType);
            entity.HasIndex(e => e.IsGlobal);
            entity.Property(e => e.CriteriaJson).HasColumnType("nvarchar(max)");
        });
        
        // Lecture configuration
        modelBuilder.Entity<Lecture>(entity =>
        {
            entity.HasIndex(e => e.CourseId);
            entity.HasIndex(e => e.LectureDate);
            entity.HasIndex(e => new { e.CourseId, e.LectureDate, e.StartTime });
            
            entity.HasOne(l => l.Course)
                  .WithMany()
                  .HasForeignKey(l => l.CourseId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        
        // LectureMaterial configuration
        modelBuilder.Entity<LectureMaterial>(entity =>
        {
            entity.HasIndex(e => e.LectureId);
            entity.HasIndex(e => e.FileType);
            
            entity.HasOne(m => m.Lecture)
                  .WithMany()
                  .HasForeignKey(m => m.LectureId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        
        // LectureAttendance configuration
        modelBuilder.Entity<LectureAttendance>(entity =>
        {
            entity.HasIndex(e => e.LectureId);
            entity.HasIndex(e => e.StudentId);
            entity.HasIndex(e => new { e.LectureId, e.StudentId }).IsUnique();
            
            entity.HasOne(a => a.Lecture)
                  .WithMany()
                  .HasForeignKey(a => a.LectureId)
                  .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(a => a.Student)
                  .WithMany()
                  .HasForeignKey(a => a.StudentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}