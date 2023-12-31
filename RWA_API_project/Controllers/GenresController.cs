﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWA_API_project.Models;

namespace RWA_API_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly RwaMoviesContext _context;
        private readonly IMapper _mapper;

        public GenresController(RwaMoviesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreVM>>> GetGenres()
        {
          if (_context.Genres == null)
          {
              return NotFound();
          }
            return _mapper.Map<List<GenreVM>>(await _context.Genres.ToListAsync());
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreVM>> GetGenre(int id)
        {
          if (_context.Genres == null)
          {
              return NotFound();
          }
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return _mapper.Map<GenreVM>(genre);
        }

        // PUT: api/Genres/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int id, GenreVM genreVm)
        {
            var genre = _mapper.Map<Genre>(genreVm);

            if (id != genre.Id)
            {
                return BadRequest();
            }

            _context.Entry(genre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
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

        // POST: api/Genres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Genre>> PostGenre(GenreVM genreVm)
        {
            var genre = _mapper.Map<Genre>(genreVm);

            if (_context.Genres == null)
            {
                return Problem("Entity set 'RwaMoviesContext.Genres'  is null.");
            }

            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGenre", new { id = genre.Id }, genre);
        }

        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            if (_context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            var referencedVideo = await _context.Videos.AnyAsync(v => v.GenreId == id);

            if (referencedVideo)
            {
                return BadRequest("This genre is referenced by video/s");
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GenreExists(int id)
        {
            return (_context.Genres?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
