using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList;
using RWA_MVC_project.Filters;
using RWA_MVC_project.Models;
using RWA_MVC_project.Repos;
using RWA_MVC_project.Services;

namespace RWA_MVC_project.Controllers
{
    [TypeFilter(typeof(LoginFilter))]
    public class VideosController : Controller
    {
        private readonly RwaMoviesContext _context;       

        public VideosController(RwaMoviesContext context)
        {
            _context = context;
        }

        // GET: Videos
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 4;
            int pageNumber = page ?? 1;

            if (Request.Cookies.ContainsKey("searchVideos"))
            {
                string searchText = Request.Cookies["searchVideos"];

                var videoQuery = _context.Videos
                    .Include(v => v.Genre)
                    .Include(v => v.Image)
                    .Where(v => v.Name.Contains(searchText) ||
                                v.Genre.Name.Contains(searchText))
                    .OrderBy(v => v.CreatedAt);

                var results = await videoQuery.ToListAsync();

                ViewData["searchVideos"] = searchText;

                return View(results.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var rwaMoviesContextPaged = await _context.Videos
                    .Include(v => v.Genre)
                    .Include(v => v.Image)
                    .OrderBy(v => v.CreatedAt)
                    .ToListAsync();

                return View(rwaMoviesContextPaged.ToPagedList(pageNumber, pageSize));
            }
        }

        // GET: Videos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Videos == null)
            {
                return NotFound();
            }

            var video = await _context.Videos
                .Include(v => v.Genre)
                .Include(v => v.Image)
                .Include(v => v.VideoTags)
                .ThenInclude(vt => vt.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // GET: Videos/Create
        [TypeFilter(typeof(AdministratorFilter))]
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Id");
            return View();
        }

        // POST: Videos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Create([Bind("Id,CreatedAt,Name,Description,GenreId,TotalSeconds,StreamingUrl,ImageId")] Video video)
        {
            ModelState.Remove("Genre");
            if (ModelState.IsValid)
            {
                _context.Add(video);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Id", video.GenreId);
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Id", video.ImageId);
            return View(video);
        }

        public IActionResult Search(string searchText, int? page)
        {
            int pageSize = 4;
            int pageNumber = page ?? 1;

            if (!string.IsNullOrEmpty(searchText))
            {
                Response.Cookies.Append("searchVideos", searchText);
            }

            searchText = !string.IsNullOrEmpty(searchText) ? searchText : Request.Cookies["searchVideos"];

            var videosQuery = _context.Videos
                .Include(v => v.Genre)
                .Include(v => v.Image)
                .Include(v => v.VideoTags)
                .Where(v => v.Name.Contains(searchText) ||
                v.Genre.Name.Contains(searchText) ||
                v.VideoTags.Any(vt => vt.Tag.Name.Contains(searchText))
                );          

            var videosPaged = videosQuery.ToPagedList(pageNumber, pageSize);

            return View("Index", videosPaged);
        }

        public IActionResult ClearSearch()
        {
            Response.Cookies.Delete("searchVideos");
            return RedirectToAction(nameof(Index));
        }

        // GET: Videos/Edit/5
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Videos == null)
            {
                return NotFound();
            }

            var video = await _context.Videos.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", video.GenreId);
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Id", video.ImageId);
            return View(video);
        }

        // POST: Videos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreatedAt,Name,Description,GenreId,TotalSeconds,StreamingUrl,ImageId")] Video video)
        {
            if (id != video.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Genre");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(video);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoExists(video.Id))
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
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Id", video.GenreId);
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Id", video.ImageId);
            return View(video);
        }

        // GET: Videos/Delete/5
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Videos == null)
            {
                return NotFound();
            }

            var video = await _context.Videos
                .Include(v => v.Genre)
                .Include(v => v.Image)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (_context.Videos == null)
                {
                    return Problem("Entity set 'RwaMoviesContext.Videos'  is null.");
                }
                var video = await _context.Videos.FindAsync(id);
                if (video != null)
                {
                    _context.Videos.Remove(video);
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

        private bool VideoExists(int id)
        {
          return (_context.Videos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
