using Microsoft.AspNetCore.Mvc;
using dotnetmvcapp.Models;
using dotnetmvcapp.Services;
using System;
using System.Threading.Tasks;

namespace dotnetmvcapp.Controllers
{
    public class SongController : Controller
    {
        private readonly ISongService _songService;

        public SongController(ISongService songService)
        {
            _songService = songService;

        }

        public IActionResult AddSong()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSong(Song song)
        {
            try
            {
                if (song == null)
                {
                    return BadRequest("Invalid Song data");
                }

                var success = _songService.AddSong(song);

                if (success)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, "Failed to add the song. Please try again.");
                return View(song);
            }
            catch (Exception ex)
            {
                // Log or print the exception to get more details
                Console.WriteLine("Exception: " + ex.Message);

                // Return an error response or another appropriate response
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again.");
                return View(song);
            }
        }

        public IActionResult Index()
        {
            try
            {
                var listSongs = _songService.GetAllSongs();
                return View("Index",listSongs);
            }
            catch (Exception ex)
            {
                // Log or print the exception to get more details
                Console.WriteLine("Exception: " + ex.Message);

                // Return an error view or another appropriate response
                return View("Error"); // Assuming you have an "Error" view
            }
        }

        public IActionResult Search(int id)
        {
            try
            {
                var song = _songService.GetSongById(id);

                if (song != null)
                {
                    return View("Search",new[] { song });
                }
                else
                {
                    return View("Search",new Song[0]);
                }
            }
            catch (Exception ex)
            {
                // Log or print the exception to get more details
                Console.WriteLine("Exception: " + ex.Message);

                // Return an error view or another appropriate response
                return View("Error"); // Assuming you have an "Error" view
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var success = _songService.DeleteSong(id);

                if (success)
                {
                    // Check if the deletion was successful and return a view or a redirect
                    return RedirectToAction("Index"); // Redirect to the list of songs, for example
                }
                else
                {
                    // Handle other error cases
                    return View("Error"); // Assuming you have an "Error" view
                }
            }
            catch (Exception ex)
            {
                // Log or print the exception to get more details
                Console.WriteLine("Exception: " + ex.Message);

                // Return an error view or another appropriate response
                return View("Error"); // Assuming you have an "Error" view
            }
        }
    }
}
