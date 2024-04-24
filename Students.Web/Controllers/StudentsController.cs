using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Students.Common.Data;
using Students.Common.Models;
using Students.Interfaces;
using Students.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Students.Web.Controllers;

public class StudentsController : Controller
{
    #region Ctor And Properties

    private readonly StudentsContext _context;
    private readonly ILogger _logger;
    private readonly ISharedResourcesService _sharedResourcesService;
    private readonly IDatabaseService _databaseService;

    public StudentsController(
        StudentsContext context,
        ILogger<StudentsController> logger,
        ISharedResourcesService sharedResourcesService,
        IDatabaseService databaseService)
    {
        _context = context;
        _logger = logger;
        _sharedResourcesService = sharedResourcesService;
        _databaseService = databaseService;
    }

    #endregion // Ctor And Properties

    #region Public Methods

    // GET: Students
    public async Task<IActionResult> Index(string? culture)
    {
        IActionResult result = View();
        try
        {
            var model = await _databaseService.IndexStudent(culture);
            result = View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception caught: " + ex.Message);
        }

        return result;
    }

    // GET: Students/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        IActionResult result = NotFound();

        try
        {
            var student = await _databaseService.DisplayStudentAsync(id);
            result = View(student);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception caught: " + ex.Message);
        }

        return result;
    }

    // GET: Students/Create
    public async Task<IActionResult> Create()
    {
        IActionResult result = View();
        try
        {
            var student = await _databaseService.CreateStudent();
            result = View(student);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception caught: " + ex.Message);
        }
        return result;
    }

    // POST: Students/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int id, string name, int age, string major, int[] subjectIdDst)
    {
        IActionResult result = View();
        try
        {
            var student = await _databaseService.SaveStudent(id, name, age, major, subjectIdDst);
            result = View(student);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception caught: " + ex.Message);
        }
        return result;
    }



    // GET: Students/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        IActionResult result = NotFound();

        try
        {
            if (id != null)
            {
                var student = await _databaseService.EditStudents(id);
                if (student != null)
                {
                    result = View(student);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception caught: " + ex.Message);
        }

        return result;
    }

    // POST: Students/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, string name, int age, string major, int[] subjectIdDst)
    {
        IActionResult result;

        try
        {
            bool saveResult = _databaseService.EditStudent(id, name, age, major, subjectIdDst);
            if (!saveResult)
            {
                throw new Exception("Error saving changes to the database.");
            }

            // Set the result to redirect to the Index action
            result = RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Log the exception and set the result to return the view with the current student
            _logger.LogError("Exception caught: " + ex.Message);
            var student = await _context.Student.FindAsync(id);
            result = View(student);
        }

        return result;
    }


    // GET: Students/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        IActionResult result = View();
        try
        {
            if (id == null)
            {
                result = NotFound();
            }
            else
            {
                var student = await _databaseService.DeleteStudent(id);
                result = View(student);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception caught: " + ex.Message);
        }
        return result;
    }

    // POST: Students/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        IActionResult result = View();
        try
        {
            var student = await _databaseService.DeleteStudents(id);
            result = RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception caught: " + ex.Message);
        }

        return result;
    }

    #endregion // Public Methods

    #region Private Methods
        
    private bool StudentExists(int id)
    {
        var result = _databaseService.StudentExist(id);
        return result;
    }

    #endregion // Private Methods
}