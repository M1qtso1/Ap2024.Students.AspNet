using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Students.Common.Models;

namespace Students.Common.Data;

public class StudentsContext : DbContext
{
    public StudentsContext(DbContextOptions<StudentsContext> options)
        : base(options)
    {

    }

    public StudentsContext()
    {
    }

    public DbSet<Student> Student { get; set; } = default!;
    public DbSet<Subject> Subject { get; set; } = default!;
    public DbSet<StudentSubject> StudentSubject { get; set; } = default!;
    public DbSet<Classroom> Classroom { get; set; } = default!;
    public DbSet<Lecturer> Lecturer { get; set; } = default!;
    public DbSet<Book> Book { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("StudentsContext");
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentSubject>()
            .HasKey(ss => new { ss.StudentId, ss.SubjectId });

        modelBuilder.Entity<StudentSubject>()
            .HasOne(ss => ss.Student)
            .WithMany(s => s.StudentSubjects)
            .HasForeignKey(ss => ss.StudentId);

        modelBuilder.Entity<StudentSubject>()
            .HasOne(ss => ss.Subject)
            .WithMany(s => s.StudentSubjects)
            .HasForeignKey(ss => ss.SubjectId);
    }
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<StudentsContext>
    {
        public StudentsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StudentsContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=StudentsProgrammingContext-23428837-2834-4740-ab78-0b481781e013;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new StudentsContext(optionsBuilder.Options);
        }
    }
}