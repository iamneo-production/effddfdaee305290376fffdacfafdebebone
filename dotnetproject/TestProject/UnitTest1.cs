using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using dotnetapiapp.Controllers;
using dotnetapiapp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
namespace dotnetapiapp.Tests
{
    [TestFixture]
    public class SongControllerTests
    {
        private SongController _songController;
        private SongDbContext _context;

        [SetUp]
        public void Setup()
        {
            // Initialize an in-memory database for testing
            var options = new DbContextOptionsBuilder<SongDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new SongDbContext(options);
            _context.Database.EnsureCreated(); // Create the database

            // Seed the database with sample data
            _context.Songs.AddRange(new List<Song>
            {
                new Song { SongID = 1, SongName = "Song 1", SingerName = "Artist 1", ReleaseYear = "2035" },
                new Song { SongID = 2, SongName = "Song 2", SingerName = "Artist 2", ReleaseYear = "2042" },
                new Song { SongID = 3, SongName = "Song 3", SingerName = "Artist 3", ReleaseYear = "1951" },
            });
            _context.SaveChanges();

            _songController = new SongController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted(); // Delete the in-memory database after each test
            _context.Dispose();
        }
        [Test]
        public void SongClassExists()
        {
            // Arrange
            Type songType = typeof(Song);

            // Act & Assert
            Assert.IsNotNull(songType, "Song class not found.");
        }
        [Test]
        public void Song_Properties_SongName_ReturnExpectedDataTypes()
        {
            // Arrange
            Song song = new Song();
            PropertyInfo propertyInfo = song.GetType().GetProperty("SongName");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "SongName property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "SongName property type is not string.");
        }
[Test]
        public void Song_Properties_SingerName_ReturnExpectedDataTypes()
        {
            // Arrange
            Song song = new Song();
            PropertyInfo propertyInfo = song.GetType().GetProperty("SingerName");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "SingerName property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "SingerName property type is not string.");
        }
        [Test]
        public void Song_Properties_ReleaseYear_ReturnExpectedDataTypes()
        {
            // Arrange
            Song song = new Song();
            PropertyInfo propertyInfo = song.GetType().GetProperty("ReleaseYear");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "ReleaseYear property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "ReleaseYear property type is not string.");
        }

        [Test]
        public async Task GetAllSongs_ReturnsOkResult()
        {
            // Act
            var result = await _songController.GetAllSongs();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetAllSongs_ReturnsAllSongs()
        {
            // Act
            var result = await _songController.GetAllSongs();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;

            Assert.IsInstanceOf<IEnumerable<Song>>(okResult.Value);
            var songs = okResult.Value as IEnumerable<Song>;

            var songCount = songs.Count();
            Assert.AreEqual(3, songCount); // Assuming you have 3 songs in the seeded data
        }

        [Test]
        public async Task GetSongById_ExistingId_ReturnsOkResult()
        {
            // Arrange
            var existingId = 1;

            // Act
            var result = await _songController.GetSongById(existingId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetSongById_ExistingId_ReturnsSong()
        {
            // Arrange
            var existingId = 1;

            // Act
            var result = await _songController.GetSongById(existingId);

            // Assert
            Assert.IsNotNull(result);

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;

            var song = okResult.Value as Song;
            Assert.IsNotNull(song);
            Assert.AreEqual(existingId, song.SongID);
        }

        [Test]
        public async Task GetSongById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingId = 99; // Assuming this ID does not exist in the seeded data

            // Act
            var result = await _songController.GetSongById(nonExistingId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task AddSong_ValidData_ReturnsOkResult()
        {
            // Arrange
            var newSong = new Song
            {
                SongName = "New Song",
                SingerName = "New Artist",
                ReleaseYear = "2030"
            };

            // Act
            var result = await _songController.AddSong(newSong);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task DeleteSong_ValidId_ReturnsNoContent()
        {
            // Arrange
              // var controller = new SongsController(context);

                // Act
                var result = await _songController.DeleteSong(1) as NoContentResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(204, result.StatusCode);
        }

        [Test]
        public async Task DeleteSong_InvalidId_ReturnsBadRequest()
        {
                   // Act
                var result = await _songController.DeleteSong(0) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(400, result.StatusCode);
                Assert.AreEqual("Not a valid song id", result.Value);
        }
    }
}
