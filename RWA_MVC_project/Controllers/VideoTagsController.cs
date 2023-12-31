﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RWA_MVC_project.Filters;
using RWA_MVC_project.Models;

namespace RWA_MVC_project.Controllers
{
    [TypeFilter(typeof(LoginFilter))]
    public class VideoTagsController : Controller
    {     
        private readonly RwaMoviesContext _context;

        public VideoTagsController(RwaMoviesContext context)
        {
            _context = context;
        }

        // GET: VideoTags
        public async Task<IActionResult> Index()
        {
            var rwaMoviesContext = _context.VideoTags.Include(v => v.Tag).Include(v => v.Video);
            return View(await rwaMoviesContext.ToListAsync());
        }

        // GET: VideoTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VideoTags == null)
            {
                return NotFound();
            }

            var videoTag = await _context.VideoTags
                .Include(v => v.Tag)
                .Include(v => v.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoTag == null)
            {
                return NotFound();
            }

            return View(videoTag);
        }

        // GET: VideoTags/Create
        [TypeFilter(typeof(AdministratorFilter))]
        public IActionResult Create()
        {
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Name");
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "Name");
            return View();
        }

        // POST: VideoTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Create([Bind("Id,VideoId,TagId")] VideoTag videoTag)
        {

            ModelState.Remove("Tag");
            ModelState.Remove("Video");
            if (ModelState.IsValid)
            {
                _context.Add(videoTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Id", videoTag.TagId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "Id", videoTag.VideoId);
            return View(videoTag);
        }

        // GET: VideoTags/Edit/5
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VideoTags == null)
            {
                return NotFound();
            }

            var videoTag = await _context.VideoTags.FindAsync(id);
            if (videoTag == null)
            {
                return NotFound();
            }
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Name", videoTag.TagId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "Name", videoTag.VideoId);
            return View(videoTag);
        }

        // POST: VideoTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VideoId,TagId")] VideoTag videoTag)
        {
            if (id != videoTag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoTagExists(videoTag.Id))
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
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Id", videoTag.TagId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "Id", videoTag.VideoId);
            return View(videoTag);
        }

        // GET: VideoTags/Delete/5
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VideoTags == null)
            {
                return NotFound();
            }

            var videoTag = await _context.VideoTags
                .Include(v => v.Tag)
                .Include(v => v.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoTag == null)
            {
                return NotFound();
            }

            return View(videoTag);
        }

        // POST: VideoTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(AdministratorFilter))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (_context.VideoTags == null)
                {
                    return Problem("Entity set 'RwaMoviesContext.VideoTags'  is null.");
                }
                var videoTag = await _context.VideoTags.FindAsync(id);
                if (videoTag != null)
                {
                    _context.VideoTags.Remove(videoTag);
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

        private bool VideoTagExists(int id)
        {
          return (_context.VideoTags?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
