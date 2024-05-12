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

namespace Students.Web.Controllers
{
    public class LecturersController : Controller
    {
        private readonly StudentsContext _context;
        private readonly ILogger _logger;
        private readonly ISharedResourcesService _sharedResourcesService;
        private readonly IDatabaseService _databaseService;

        public LecturersController(StudentsContext context,
        ILogger<StudentsController> logger,
        ISharedResourcesService sharedResourcesService,
        IDatabaseService databaseService)
        {
            _context = context;
            _logger = logger;
            _sharedResourcesService = sharedResourcesService;
            _databaseService = databaseService;
        }

        // GET: Lecturers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lecturer.ToListAsync());
        }

        // GET: Lecturers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var lecturer = await _databaseService.DetailsLecturer(id);
            var result = View(lecturer);
            if (lecturer == null)
            {
                return NotFound();
            }
            return result;
        }

        // GET: Lecturers/Create
        public async Task<IActionResult> Create()
        {
            var lecturer = await _databaseService.CreateLecturer();
            var result = View(lecturer);
            return result;
        }

        // POST: Lecturers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Age")] Lecturer lecturer, int[] subjectIdDst)
        {
            IActionResult result = View();
            if (ModelState.IsValid)
            {
                _context.Add(lecturer);
                await _databaseService.SaveLecturer(lecturer, subjectIdDst);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("AvailableSubjects", "error ");
                result = View(await _databaseService.CreateLecturer());
            }
            return result;
        }

        // GET: Lecturers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _databaseService.EditLecturer(id);
            var result = View(lecturer);
            if (lecturer == null)
            {
                return NotFound();
            }
            return result;
        }

        // POST: Lecturers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Lecturer lecturer, int[] subjectIdDst)
        {
            IActionResult result = View();
            if (id != lecturer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _databaseService.EditLecturers(id, subjectIdDst, lecturer);
                    result = View(lecturer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LecturerExists(lecturer.Id))
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
            else
            {
                ModelState.AddModelError("AvailableSubjects", "error ");
                result = View(await _databaseService.EditLecturer(id));
            }
            return result;
        }

        // GET: Lecturers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _databaseService.DeleteLecturer(id);
            var result = View(lecturer);
            if (lecturer == null)
            {
                return NotFound();
            }

            return result;
        }

        // POST: Lecturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lecturer = await _databaseService.DeleteConfirmedLecturer(id);
            var result = RedirectToAction(nameof(Index));
            return result;
        }

        private bool LecturerExists(int id)
        {
            var result = _databaseService.LecturerExist(id);
            return result;
        }
    }
}
