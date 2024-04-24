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
            var additionResult = await _context.SaveChangesAsync();
            if (additionResult == 0)
            {
                throw new Exception("Error saving changes to the database.");
            }
            foreach (var chosenSubject in chosenSubjects)
            {
                student.AddSubject(chosenSubject);
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
}