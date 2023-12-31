﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWA_MVC_project.Filters;
using RWA_MVC_project.Models;
using X.PagedList;

namespace RWA_MVC_project.Controllers
{
    [TypeFilter(typeof(LoginFilter))]
    public class GenresController : Controller
    {
        private readonly RwaMoviesContext _context;

        public GenresController(RwaMoviesContext context)
        {
            _context = context;
        }

        // GET: Genres
        public async Task<IActionResult> Index(int? page)
        {
            const int pageSize = 3;

            ViewData["page"] = page ?? 1;
            ViewData["size"] = pageSize;
            ViewData["pages"] = _context.Genres.Count() / pageSize;

            var genres = await _context.Genres
                        .OrderBy(g => g.Name)
                        .ToPagedListAsync(page ?? 1, pageSize);

            return View(genres);
        }

        public async Task<IActionResult> GenrePagingPartial(int? page)
        {
            const int pageSize = 3;

            ViewData["page"] = page ?? 1;
            ViewData["size"] = pageSize;
            ViewData["pages"] = _context.Genres.Count() / pageSize;

            var genres = await _context.Genres
                        .OrderBy(g => g.Name)
                        .ToPagedListAsync(page ?? 1, pageSize);

            return PartialView("_GenrePagingPartial", genres);
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // GET: Genres/Create
        [TypeFilter(typeof(AdministratorFilter))]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        public IActionResult Search(string searchText)
        {
            var genres = _context.Genres
                .Where(g => g.Name.Contains(searchText) ||
                g.Description.Contains(searchText));

            return View("Index", genres);
        }

        // GET: Genres/Edit/5
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Genre genre)
        {
            if (id != genre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(genre.Id))
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
            return View(genre);
        }

        // GET: Genres/Delete/5
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (_context.Genres == null)
                {
                    return Problem("Entity set 'RwaMoviesContext.Genres'  is null.");
                }
                var genre = await _context.Genres.FindAsync(id);
                if (genre != null)
                {
                    _context.Genres.Remove(genre);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        private bool GenreExists(int id)
        {
          return (_context.Genres?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
