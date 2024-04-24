using Students.Common.Models;
using System.Runtime.InteropServices;

namespace Students.Interfaces;

public interface IDatabaseService
{
    bool EditStudent(int id, string name, int age, string major, int[] subjectIdDst);

    Task<Student?> DisplayStudentAsync(int? id);
    Task<Student?> CreateStudent();
    Task<List<Student>?> IndexStudent(string? culture);
    Task<Student?> SaveStudent(int id, string name, int age, string major, int[] subjectIdDst);
    Task<Student?> EditStudents(int? id);
    Task<Student?> DeleteStudent(int? id);
    Task<Student?> DeleteStudents(int? id);
    bool StudentExist(int id);
    Task<Subject?> DetailsSubject(int? id);
    Task<List<Subject>?> IndexSubject();
    Task<Subject?> CreateSubjects(Subject subject);
    Task<Subject?> EditSubject(int? id);
    Task<Subject?> EditSubjects(int id, Subject subject);
    Task<Subject?> DeleteSubject(int? id);
    Task<Subject?> DeleteSubjects(int id);
    bool SubjectExist(int id);
}