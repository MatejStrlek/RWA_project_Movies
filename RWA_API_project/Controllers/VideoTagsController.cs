using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWA_API_project.Models;

namespace RWA_API_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoTagsController : ControllerBase
    {
        private readonly RwaMoviesContext _context;

        public VideoTagsController(RwaMoviesContext context)
        {
            _context = context;
        }

        // GET: api/VideoTags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VideoTag>>> GetVideoTags()
        {
          if (_context.VideoTags == null)
          {
              return NotFound();
          }
            return await _context.VideoTags.ToListAsync();
        }

        // GET: api/VideoTags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoTag>> GetVideoTag(int id)
        {
          if (_context.VideoTags == null)
          {
              return NotFound();
          }
            var videoTag = await _context.VideoTags.FindAsync(id);

            if (videoTag == null)
            {
                return NotFound();
            }

            return videoTag;
        }

        // PUT: api/VideoTags/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVideoTag(int id, VideoTag videoTag)
        {
            if (id != videoTag.Id)
            {
                return BadRequest();
            }

            _context.Entry(videoTag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoTagExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/VideoTags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VideoTag>> PostVideoTag(VideoTag videoTag)
        {
          if (_context.VideoTags == null)
          {
              return Problem("Entity set 'RwaMoviesContext.VideoTags'  is null.");
          }
            _context.VideoTags.Add(videoTag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVideoTag", new { id = videoTag.Id }, videoTag);
        }

        // DELETE: api/VideoTags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoTag(int id)
        {
            if (_context.VideoTags == null)
            {
                return NotFound();
            }
            var videoTag = await _context.VideoTags.FindAsync(id);
            if (videoTag == null)
            {
                return NotFound();
            }

            _context.VideoTags.Remove(videoTag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VideoTagExists(int id)
        {
            return (_context.VideoTags?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
