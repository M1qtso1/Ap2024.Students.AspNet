using Students.Common.Models;

namespace Students.Interfaces;

public interface IDatabaseService
{
    bool EditStudent(int id, string name, int age, string major, int[] subjectIdDst);

    Task<Student?> DisplayStudentAsync(int? id);
    Task<Student?> CreateStudent();
    Task<List<Student>?> IndexStudent(string? culture);
}