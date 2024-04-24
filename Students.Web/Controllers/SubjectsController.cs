using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Students.Common.Data;
using Students.Common.Models;
using Students.Interfaces;
using Students.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Students.Web.Controllers;

public class SubjectsController : Controller
{
    private readonly StudentsContext _context;

    private readonly IDatabaseService _databaseService;

    public SubjectsController(StudentsContext context,
        IDatabaseService databaseService)
    {
        _context = context;
        _databaseService = databaseService;
    }

    // GET: Subjects
    public async Task<IActionResult> Index()
    {
        return View(await _databaseService.IndexSubject());
    }

    // GET: Subjects/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var subject = await _databaseService.DetailsSubject(id);
        var result = subject;
        if (subject == null)
        {
            return NotFound();
        }

        return View(result);
    }

    // GET: Subjects/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Subjects/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Credits")] Subject subject)
    {
        IActionResult result = NotFound();
        if (ModelState.IsValid)
        {
            await _databaseService.CreateSubjects(subject) ;
            result = View(subject);
            return RedirectToAction(nameof(Index));
        }
        return result;
    }

    // GET: Subjects/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var subject = await _databaseService.EditSubject(id);
        var result = View(subject);
        if (subject == null)
        {
            return NotFound();
        }
        return result;
    }

    // POST: Subjects/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Credits")] Subject subject)
    {
        IActionResult result = NotFound();
        if (id != subject.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _databaseService.EditSubjects(id, subject) ;
                result = View(subject);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(subject.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return result;
    }

    // GET: Subjects/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        IActionResult result = NotFound();
        if (id == null)
        {
            return NotFound();
        }

        var subject = await _databaseService.DeleteSubject(id);
        result = View(subject);
        if (subject == null)
        {
            return NotFound();
        }

        return result;
    }

    // POST: Subjects/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var subject = await _databaseService.DeleteSubjects(id);
        var result =  RedirectToAction(nameof(Index));
        return result;
    }

    private bool SubjectExists(int id)
    {
        var result = _databaseService.SubjectExist(id);
        return result;
    }
}