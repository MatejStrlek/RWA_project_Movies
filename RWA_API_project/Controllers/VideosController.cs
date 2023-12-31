﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWA_API_project.Models;

namespace RWA_API_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly RwaMoviesContext _context;
        private readonly IMapper _mapper;

        public VideosController(RwaMoviesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Videos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VideoVM>>> GetVideos()
        {
          if (_context.Videos == null)
          {
              return NotFound();
          }

          var videos = await _context.Videos
                .OrderBy(x => x.Id)
                .ThenBy(x => x.Name)
                .Include(x => x.TotalSeconds)
                .ToListAsync();

            return _mapper.Map<List<VideoVM>>(videos);
        }

        // GET: api/Videos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoVM>> GetVideo(int id)
        {
          if (_context.Videos == null)
          {
              return NotFound();
          }
            var video = await _context.Videos.FindAsync(id);

            if (video == null)
            {
                return NotFound();
            }

            return _mapper.Map<VideoVM>(video);
        }

        // PUT: api/Videos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVideo(int id, VideoVM videoVm)
        {
            var video = _mapper.Map<Video>(videoVm);

            if (id != video.Id)
            {
                return BadRequest();
            }

            _context.Entry(video).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoExists(id))
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

        // POST: api/Videos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Video>> PostVideo(VideoVM videoVm)
        {
            if (_context.Videos == null)
            {
                return Problem("Entity set 'RwaMoviesContext.Videos'  is null.");
            }

            var video = _mapper.Map<Video>(videoVm);
            _context.Videos.Add(video);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVideo", new { id = video.Id }, video);
        }

        // DELETE: api/Videos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            if (_context.Videos == null)
            {
                return NotFound();
            }
            var video = await _context.Videos.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }

            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VideoExists(int id)
        {
            return (_context.Videos?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<VideoVM>>> Search(int page, int size, string? filter)
        {
            IEnumerable<Video> filtered;

            var videos = await _context.Videos.ToListAsync();

            if (filter != null)
            {
                filtered = videos.Where(x =>
                    x.Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                filtered = videos;
            }

            var results = filtered.Skip((page - 1) * size).Take(size);

            return Ok(_mapper.Map<List<VideoVM>>(results));
        }
    }
}
