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
using NuGet.DependencyResolver;

namespace Students.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly StudentsContext _context;
        private readonly ILogger _logger;
        private readonly ISharedResourcesService _sharedResourcesService;
        private readonly IDatabaseService _databaseService;

        public BooksController(StudentsContext context,
        ILogger<StudentsController> logger,
        ISharedResourcesService sharedResourcesService,
        IDatabaseService databaseService)
        {
            _context = context;
        _logger = logger;
        _sharedResourcesService = sharedResourcesService;
        _databaseService = databaseService;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Book.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _databaseService.DetailsBooks(id);
            var result = View(book);
            if (book == null)
            {
                return NotFound();
            }

            return result;
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,Description")] Book book)
        {

            IActionResult result = View();
            if (ModelState.IsValid)
            {
                await _databaseService.CreateBooks(book);
                result = View(book);
                return RedirectToAction(nameof(Index));
            }
            return result;
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _databaseService.EditBooks(id);
            var result = View(book);
            if (book == null)
            {
                return NotFound();
            }
            return result;
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Description")] Book book)
        {

            IActionResult result = View();
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _databaseService.EditBook(id, book);
                    result = View(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _databaseService.DeleteBooks(id);
            var result = View(book);
            if (book == null)
            {
                return NotFound();
            }

            return result;
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _databaseService.DeleteConfirmedBook(id);
            var result = RedirectToAction(nameof(Index));
            return result;
        }

        private bool BookExists(int id)
        {
            var result = _databaseService.BookExist(id);
            return result;
        }
    }
}
