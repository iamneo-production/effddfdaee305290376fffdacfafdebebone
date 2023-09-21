using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using dotnetmvcapp.Controllers;
using dotnetmvcapp.Models;
using dotnetmvcapp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;
using Moq;

namespace dotnetmvcapp.Tests
{
    [TestFixture]
    public class SongControllerTests
    {
        private Mock<ISongService> mockSongService;
        private SongController controller;
        [SetUp]
        public void Setup()
        {
            mockSongService = new Mock<ISongService>();
            controller = new SongController(mockSongService.Object);
        }

        [Test]
        public void AddSong_ValidData_SuccessfulAddition_RedirectsToIndex()
        {
            // Arrange
            var mockSongService = new Mock<ISongService>();
            mockSongService.Setup(service => service.AddSong(It.IsAny<Song>())).Returns(true);
            var controller = new SongController(mockSongService.Object);
            var song = new Song(); // Provide valid song data

            // Act
            var result = controller.AddSong(song) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }
        [Test]
        public void AddSong_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var mockSongService = new Mock<ISongService>();
            var controller = new SongController(mockSongService.Object);
            Song invalidSong = null; // Invalid song data

            // Act
            var result = controller.AddSong(invalidSong) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Invalid Song data", result.Value);
        }
        [Test]
        public void AddSong_FailedAddition_ReturnsViewWithModelError()
        {
            // Arrange
            var mockSongService = new Mock<ISongService>();
            mockSongService.Setup(service => service.AddSong(It.IsAny<Song>())).Returns(false);
            var controller = new SongController(mockSongService.Object);
            var song = new Song(); // Provide valid song data

            // Act
            var result = controller.AddSong(song) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
            // Check for expected model state error
            Assert.AreEqual("Failed to add the song. Please try again.", controller.ModelState[string.Empty].Errors[0].ErrorMessage);
        }


        [Test]
        public void AddSong_Post_ValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockSongService = new Mock<ISongService>();
            mockSongService.Setup(service => service.AddSong(It.IsAny<Song>())).Returns(true);
            var controller = new SongController(mockSongService.Object);
            var song = new Song();

            // Act
            var result = controller.AddSong(song) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void AddSong_Post_InvalidModel_ReturnsViewResult()
        {
            // Arrange
            var mockSongService = new Mock<ISongService>();
            var controller = new SongController(mockSongService.Object);
            controller.ModelState.AddModelError("error", "Error");
            var song = new Song();

            // Act
            var result = controller.AddSong(song) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(song, result.Model);
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var mockSongService = new Mock<ISongService>();
            mockSongService.Setup(service => service.GetAllSongs()).Returns(new List<Song>());
            var controller = new SongController(mockSongService.Object);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ViewName);
        }

        [Test]
        public void Search_ValidId_ReturnsViewResult()
        {
            // Arrange
            var mockSongService = new Mock<ISongService>();
            var expectedSong = new Song { SongID = 1, SongName = "SongTitle" };
            mockSongService.Setup(service => service.GetSongById(1)).Returns(expectedSong);
            var controller = new SongController(mockSongService.Object);

            // Act
            var result = controller.Search(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Search", result.ViewName);
            var model = result.Model as Song[];
            Assert.IsNotNull(model);
            Assert.AreEqual(expectedSong, model[0]);
        }

        [Test]
        public void Search_InvalidId_ReturnsViewResult()
        {
            // Arrange
            var mockSongService = new Mock<ISongService>();
            mockSongService.Setup(service => service.GetSongById(It.IsAny<int>())).Returns((Song)null);
            var controller = new SongController(mockSongService.Object);

            // Act
            var result = controller.Search(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Search", result.ViewName);
            var model = result.Model as Song[];
            Assert.IsNotNull(model);
            Assert.IsEmpty(model);
        }

        [Test]
        public void Delete_ValidId_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockSongService = new Mock<ISongService>();
            mockSongService.Setup(service => service.DeleteSong(1)).Returns(true);
            var controller = new SongController(mockSongService.Object);

            // Act
            var result = controller.Delete(1) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void Delete_InvalidId_ReturnsViewResult()
        {
            // Arrange
            var mockSongService = new Mock<ISongService>();
            mockSongService.Setup(service => service.DeleteSong(1)).Returns(false);
            var controller = new SongController(mockSongService.Object);

            // Act
            var result = controller.Delete(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Error", result.ViewName);
        }
    }
}
