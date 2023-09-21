using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnetapiapp.Models;

namespace dotnetapiapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly SongDbContext _context;

        public SongController(SongDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Song>>> GetAllSongs()
        {
            var songs = await _context.Songs.ToListAsync();
            return Ok(songs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Song>> GetSongById(int id)
        {
            var song = await _context.Songs.FindAsync(id);

            if (song == null)
            {
                return NotFound();
            }

            return Ok(song);
        }
        [HttpPost]
        public async Task<ActionResult> AddSong(Song song)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return detailed validation errors
            }
            await _context.Songs.AddAsync(song);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid song id");

            var song = await _context.Songs.FindAsync(id);
              _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
