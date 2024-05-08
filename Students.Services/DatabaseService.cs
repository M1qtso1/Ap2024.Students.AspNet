using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Students.Common.Data;
using Students.Common.Models;
using Students.Interfaces;
using System.Runtime.InteropServices;

namespace Students.Services;

public class DatabaseService : IDatabaseService
{
    #region Ctor and Properties

    private readonly StudentsContext _context;
    private readonly ILogger<DatabaseService> _logger;

    public DatabaseService(
        ILogger<DatabaseService> logger,
        StudentsContext context)
    {
        _logger = logger;
        _context = context;
    }

    #endregion // Ctor and Properties

    #region Public Methods

    public async Task<Student?> EditStudents(int? id)
    {
        Student? student = new Student();
        try
        {
            if (id != null)
            {
                student = await _context.Student.FindAsync(id);
                if (student != null)
                {
                    var chosenSubjects = _context.StudentSubject
                        .Where(ss => ss.StudentId == id)
                        .Select(ss => ss.Subject)
                        .ToList();
                    var availableSubjects = _context.Subject
                        .Where(s => !chosenSubjects.Contains(s))
                        .ToList();
                    student.StudentSubjects = _context.StudentSubject
                        .Where(x => x.StudentId == id)
                        .ToList();
                    student.AvailableSubjects = availableSubjects;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception caught in SaveStudents: " + ex.Message);
        }
        return student;
    }

    public bool EditStudent(int id, string name, int age, string major, int[] subjectIdDst)
    {
        var result = false;

        // Find the student
        var student = _context.Student.Find(id);
        if (student != null)
        {
            // Update the student's properties
            student.Name = name;
            student.Age = age;
            student.Major = major;

            // Get the chosen subjects
            var chosenSubjects = _context.Subject
                .Where(s => subjectIdDst.Contains(s.Id))
                .ToList();

            // Remove the existing StudentSubject entities for the student
            var studentSubjects = _context.StudentSubject
                .Where(ss => ss.StudentId == id)
                .ToList();
            _context.StudentSubject.RemoveRange(studentSubjects);

            // Add new StudentSubject entities for the chosen subjects
            foreach (var subject in chosenSubjects)
            {
                var studentSubject = new StudentSubject
                {
                    Student = student,
                    Subject = subject
                };
                _context.StudentSubject.Add(studentSubject);
            }

            // Save changes to the database
            var resultInt = _context.SaveChanges();
            result = resultInt > 0;
        }

        return result;
    }

    public async Task<Student?> DisplayStudentAsync(int? id)
    {
        Student? student = null;
        try
        {
            student = await _context.Student
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student is not null)
            {
                var studentSubjects = _context.StudentSubject
                    .Where(ss => ss.StudentId == id)
                    .Include(ss => ss.Subject)
                    .ToList();
                student.StudentSubjects = studentSubjects;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception caught in DisplayStudent: " + ex);
        }

        return student;
    }

    public async Task<Student?> CreateStudent()
    {
        var newStudent = new Student();
        try
        {
            var listOfSubjects = await _context.Subject
                .ToListAsync();
            newStudent.AvailableSubjects = listOfSubjects;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception caught in CreateStudent: " + ex.Message);
        }
        return newStudent;
    }

    public async Task<List<Student>?> IndexStudent(string? culture)
    {
        var model = new List<Student>();
        try
        {
            model = await _context.Student.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception caught in IndexStudent: " + ex.Message);
        }
        return model;
    }

    public async Task<Student?> SaveStudent(int id, string name, int age, string major, int[] subjectIdDst)
    {
        Student? student = null;
        try
        {
            var chosenSubjects = await _context.Subject
            .Where(s => subjectIdDst.Contains(s.Id))
            .ToListAsync();

            var availableSubjects = await _context.Subject
            .Where(s => !subjectIdDst.Contains(s.Id))
            .ToListAsync();

            student = new Student()
            {
                Id = id,
                Name = name,
                Age = age,
                Major = major,
                AvailableSubjects = availableSubjects
            };
            _context.Add(student);

            foreach (var chosenSubject in chosenSubjects)
            {
                student.AddSubject(chosenSubject);
            }

            var additionResult = await _context.SaveChangesAsync();
            if (additionResult == 0)
            {
                throw new Exception("Error saving changes to the database.");
            }

        }
        catch (Exception ex)
        {
            _logger.LogError("Exception caught in SaveStudent: " + ex.Message);
        }
        return student;
    }

    public async Task<Student?> DeleteStudent(int? id)
    {
        Student? student = new Student();
        try
        {
            student = await _context.Student
                    .FirstOrDefaultAsync(m => m.Id == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception caught in DeleteStudent: " + ex.Message);
        }
        return student;
    }

    public async Task<Student?> DeleteStudents(int? id)
    {
        Student? student = new Student();
        try
        {
            student = await _context.Student.FindAsync(id);
            if (student != null)
            {
                _context.Student.Remove(student);
            }
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception caught: " + ex.Message);
        }

        return student;
        #endregion // Public Methods
    }
    public bool StudentExist(int id)
    {
        var result = _context.Student.Any(e => e.Id == id);
        return result;
    }

    public async Task<Subject?> DetailsSubject(int? id)
    {
        var subject = await _context.Subject
            .FirstOrDefaultAsync(m => m.Id == id);
        return subject;
    }

    public async Task<List<Subject>?> IndexSubject()
    {
        return await _context.Subject.ToListAsync();
    }

    public async Task<Subject?> CreateSubjects(Subject subject)
    {
        _context.Add(subject);
        await _context.SaveChangesAsync();
        return subject;
    }

    public async Task<Subject?> EditSubject(int? id)
    {
        var subject = await _context.Subject.FindAsync(id);
        return subject;
    }

    public async Task<Subject?> EditSubjects(int id, Subject subject)
    {
        _context.Update(subject);
        await _context.SaveChangesAsync();
        return subject;
    }

    public async Task<Subject?> DeleteSubject(int? id)
    {
        var subject = await _context.Subject
            .FirstOrDefaultAsync(m => m.Id == id);
        return subject;
    }

    public async Task<Subject?> DeleteSubjects(int id)
    {
        var subject = await _context.Subject.FindAsync(id);
        if (subject != null)
        {
            _context.Subject.Remove(subject);
            await _context.SaveChangesAsync();
        }
        return subject;
        }
    public bool SubjectExist(int id)
    {
        var result = _context.Subject.Any(e => e.Id == id);
        return result;
    }

    public async Task<Book?> DetailsBooks(int? id)
    {
        var book = await _context.Book
            .FirstOrDefaultAsync(m => m.Id == id);
        return book ;
    }

    public async Task<Book?> CreateBooks(Book book)
    {
        _context.Add(book);
            await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book?> EditBooks(int? id)
    {
        var book = await _context.Book.FindAsync(id);
        return book;
    }

    public async Task<Book?> EditBook(int? id, Book book)
    {
        _context.Update(book);
        await _context.SaveChangesAsync();
        return book;
    }
    public async Task<Book?> DeleteBooks(int? id)
    {
        var book = await _context.Book
            .FirstOrDefaultAsync(m => m.Id == id);
        return book;
    }
    public async Task<Book?> DeleteConfirmedBook(int id)
    {
        await _context.SaveChangesAsync();
        var book = await _context.Book.FindAsync(id);
        if (book != null)
        {
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
        }
        return book;
    }
    public bool BookExist(int id)
    {
        var result = _context.Book.Any(e => e.Id == id);
        return result;
    }
    public async Task<Classroom?> DetailsClassrooms(int? id)
    {
        var classroom = await _context.Classroom
            .FirstOrDefaultAsync(m => m.Id == id);
        return classroom;
    }
    public async Task<Classroom?> CreateClassroom(Classroom classroom)
    {
        _context.Add(classroom);
            await _context.SaveChangesAsync();

        return classroom;
    }
    public async Task<Classroom?> EditClassroom(int? id)
    {
        var classroom = await _context.Classroom.FindAsync(id);
        return classroom;
    }
    public async Task<Classroom?> EditClassrooms(int id,Classroom classroom)
    {
        _context.Update(classroom);
                await _context.SaveChangesAsync();
        return classroom;
    }
    public async Task<Classroom?> DeleteClassroom(int? id)
    {
        var classroom = await _context.Classroom
            .FirstOrDefaultAsync(m => m.Id == id);
        return classroom;
    }
    public async Task<Classroom?> DeleteConfirmedClassroom(int id)
    {
        var classroom = await _context.Classroom.FindAsync(id);
        if (classroom != null)
        {
            _context.Classroom.Remove(classroom);
            await _context.SaveChangesAsync();
        }
        return classroom;
    }
    public bool ClassroomExist(int id)
    {
        return _context.Classroom.Any(e => e.Id == id);
    }

    public async Task<Lecturer?> DetailsLecturer(int? id)
    {

        var lecturer = await _context.Lecturer.Include(x => x.Subjects)
            .FirstOrDefaultAsync(m => m.Id == id);
        return lecturer;
    }

    public async Task<Lecturer?> CreateLecturer()
    {
        var lecturer = new Lecturer();
        lecturer.AvailableSubjects = await _context.Subject.ToListAsync();
        return lecturer;
    }
    public async Task<Lecturer?> SaveLecturer(Lecturer lecturer, int[] subjectIdDst)
    {
        var chosenSubjects = await _context.Subject
        .Where(s => subjectIdDst.Contains(s.Id))
        .ToListAsync();
        if (chosenSubjects.Count > 0)
        {
            lecturer.Subjects = chosenSubjects;
        }
        else
        {
            lecturer.AvailableSubjects = _context.Subject.ToList();
        }
        _context.Add(lecturer);
            await _context.SaveChangesAsync();
        
        return lecturer;
    }
    public async Task<Lecturer?> EditLecturer(int? id)
    {
        var lecturer = await _context.Lecturer.FindAsync(id);
        return lecturer;
    }
    public async Task<Lecturer?> EditLecturers(int id, Lecturer lecturer)
    {
                _context.Update(lecturer);
                await _context.SaveChangesAsync();
        return lecturer;
    }
    public async Task<Lecturer?> DeleteLecturer(int? id)
    {

        var lecturer = await _context.Lecturer
            .FirstOrDefaultAsync(m => m.Id == id);

        return lecturer;
    }
    public async Task<Lecturer?> DeleteConfirmedLecturer(int id)
    {
        var lecturer = await _context.Lecturer.Include(x=>x.Subjects).SingleOrDefaultAsync(x=>x.Id == id);
        if (lecturer != null)
        {
            _context.Lecturer.Remove(lecturer);
        }

        await _context.SaveChangesAsync();
        return lecturer;
    }
    public bool LecturerExist(int id)
    {
        return _context.Lecturer.Any(e => e.Id == id);
    }
}