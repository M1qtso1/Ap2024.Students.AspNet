using Students.Common.Models;
using System.Runtime.InteropServices;

namespace Students.Interfaces;

public interface IDatabaseService
{
    bool EditStudent(int id, string name, int age, string major, string postalcode, int[] subjectIdDst);

    Task<Student?> DisplayStudentAsync(int? id);
    Task<Student?> CreateStudent();
    Task<List<Student>?> IndexStudent(string? culture);
    Task<Student?> SaveStudent(int id, string name, int age, string major, string PostalCode, int[] subjectIdDst);
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
    Task<Book?> DetailsBooks(int? id);
    Task<Book?> CreateBooks(Book book);

    Task<Book?> EditBooks(int? id);
    Task<Book?> EditBook(int? id, Book book);
    Task<Book?> DeleteBooks(int? id);
    Task<Book?> DeleteConfirmedBook(int id);
    bool BookExist(int id);
    Task<Classroom?> DetailsClassrooms(int? id);
    Task<Classroom?> CreateClassroom(Classroom classroom); 
    Task<Classroom?> EditClassroom(int? id);
    Task<Classroom?> EditClassrooms(int id, Classroom classroom);
    Task<Classroom?> DeleteClassroom(int? id);
    Task<Classroom?> DeleteConfirmedClassroom(int id);
    bool ClassroomExist(int id);
    Task<Lecturer?> DetailsLecturer(int? id);
    Task<Lecturer?> CreateLecturer();
    Task<Lecturer?> SaveLecturer(Lecturer lecturer, int[] subjectIdDst);
    Task<Lecturer?> EditLecturer(int? id);
    Task<Lecturer?> EditLecturers(int id, Lecturer lecturer);
    Task<Lecturer?> DeleteLecturer(int? id);
    Task<Lecturer?> DeleteConfirmedLecturer(int id);
    bool LecturerExist(int id);
}