using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Students.Common.Data;
using Students.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Students.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.DependencyResolver;
using Students.Interfaces;

namespace Students.Web.Controllers
{
    public class ClassroomsController : Controller
    {
        private readonly StudentsContext _context;
        private readonly ILogger _logger;
        private readonly ISharedResourcesService _sharedResourcesService;
        private readonly IDatabaseService _databaseService;

        public ClassroomsController(StudentsContext context,
        ILogger<StudentsController> logger,
        ISharedResourcesService sharedResourcesService,
        IDatabaseService databaseService)
        {
            _context = context;
            _logger = logger;
            _sharedResourcesService = sharedResourcesService;
            _databaseService = databaseService;
        }

        // GET: Classrooms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Classroom.ToListAsync());
        }

        // GET: Classrooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroom = await _databaseService.DetailsClassrooms(id);
            var result = View(classroom);
            if (classroom == null)
            {
                return NotFound();
            }

            return result;
        }

        // GET: Classrooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Classrooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Floor,Capacity")] Classroom classroom)
        {

            IActionResult result = View();
            if (ModelState.IsValid)
            {
                await _databaseService.CreateClassroom(classroom);
                result = View(classroom);
                return RedirectToAction(nameof(Index));
            }
            return result;
        }

        // GET: Classrooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroom = await _databaseService.EditClassroom(id);
            var result = View(classroom);
            if (classroom == null)
            {
                return NotFound();
            }
            return result;
        }

        // POST: Classrooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Floor,Capacity")] Classroom classroom)
        {
            IActionResult result = View();
            if (id != classroom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _databaseService.EditClassrooms(id, classroom);
                    result = View(classroom);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassroomExists(classroom.Id))
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

        // GET: Classrooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroom = await _databaseService.DeleteClassroom(id);
            var result = View(classroom);
            if (classroom == null)
            {
                return NotFound();
            }

            return result;
        }

        // POST: Classrooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classroom = await _databaseService.DeleteConfirmedClassroom(id);
            var result = RedirectToAction(nameof(Index));
            return result;
        }

        private bool ClassroomExists(int id)
        {
            var result = _databaseService.ClassroomExist(id);
            return result;
        }
    }
}
