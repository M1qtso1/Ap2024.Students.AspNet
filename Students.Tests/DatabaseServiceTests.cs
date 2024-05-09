using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Students.Common.Data;
using Students.Common.Models;
using Students.Services;
using Xunit;

namespace Students.Tests;

public class DatabaseServiceTests
{
    [Fact]
    public void EditStudent_UpdatesStudentAndSubjects()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<StudentsContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        Mock<ILogger<DatabaseService>> logger = new();

        using var context = new StudentsContext(options);
        var service = new DatabaseService(logger.Object, context);

        var student = new Student { Id = 1, Name = "Test", Age = 20, Major = "Test Major", PostalCode = "00-000" };
        context.Student.Add(student);
        context.SaveChanges();

        var subject1 = new Subject { Id = 1, Name = "Subject1", Credits = 1, StartDate = DateTime.Now.AddDays(-7), EndDate = DateTime.Now.AddDays(-3)};
        var subject2 = new Subject { Id = 2, Name = "Subject2", Credits = 2, StartDate = DateTime.Now.AddDays(-6), EndDate = DateTime.Now.AddDays(-4)};
        context.Subject.AddRange(subject1, subject2);
        context.SaveChanges();

        // Act
        var result = service.EditStudent(student.Id, "New Name", 21, "New Major", "21-001", new[] { subject1.Id, subject2.Id });

        // Assert
        Assert.True(result);
        var updatedStudent = context.Student.Find(student.Id);
        Assert.NotNull(updatedStudent);
        Assert.Equal("New Name", updatedStudent.Name);
        Assert.Equal(21, updatedStudent.Age);
        Assert.Equal("New Major", updatedStudent.Major);
        var studentSubjects = context.StudentSubject.Where(ss => ss.StudentId == student.Id).ToList();
        Assert.Contains(studentSubjects, ss => ss.SubjectId == subject1.Id);
        Assert.Contains(studentSubjects, ss => ss.SubjectId == subject2.Id);
    }
}