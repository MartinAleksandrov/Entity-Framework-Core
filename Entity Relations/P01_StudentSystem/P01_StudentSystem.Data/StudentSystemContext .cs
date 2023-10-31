using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using StudentSystem.Common;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext :DbContext
    {
        public StudentSystemContext()
        {
                
        }

        public StudentSystemContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<StudentCourses> StudentCourses { get; set; } = null!;
        public DbSet<Resource> Resources { get; set; } = null!;
        public DbSet<Homework> Homeworks { get; set; } = null!;



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(s =>
            {
                s.Property(s => s.Name)
                .IsUnicode(true);

                s.Property<int>(s => s.PhoneNumber)
               .HasMaxLength(10)
               .IsFixedLength()
               .IsUnicode(false);
            });

            modelBuilder.Entity<Course>(c =>
            {
                c.Property(c => c.Name)
                .IsUnicode(true);

                c.Property(c => c.Description)
                .IsUnicode(true);
            });

            modelBuilder.Entity<Resource>(r =>
            {
                r.Property(r => r.Name)
                .IsUnicode(true);

                r.Property(r => r.Url)
                .IsUnicode(false);
            });

            modelBuilder.Entity<Homework>(h =>
            {
                h.Property(h => h.Content)
                .IsUnicode(false);
            });

            modelBuilder.Entity<StudentCourses>(sc =>
            {
                sc.HasKey(pk => new { pk.StudentId, pk.CourseId });
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}