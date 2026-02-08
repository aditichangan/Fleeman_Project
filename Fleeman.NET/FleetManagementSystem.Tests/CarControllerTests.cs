using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FleetManagementSystem.Api.Controllers;
using FleetManagementSystem.Api.Models;
using FleetManagementSystem.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using FluentAssertions;

namespace FleetManagementSystem.Tests
{
    public class CarControllerTests
    {
        private readonly Mock<ICarService> _mockCarService;
        private readonly Mock<IExcelUploadService> _mockExcelService;
        private readonly CarController _controller;

        public CarControllerTests()
        {
            _mockCarService = new Mock<ICarService>();
            _mockExcelService = new Mock<IExcelUploadService>();
            _controller = new CarController(_mockCarService.Object, _mockExcelService.Object);
        }

        [Fact]
        public async Task GetCarDetailsByHubAddress_ReturnsOkResult_WithCars()
        {
            // Arrange
            var hubAddress = "123 Main St";
            var expectedCars = new List<object[]>
            {
                new object[] { 1, "Toyota Camry" },
                new object[] { 2, "Honda Accord" }
            };
            _mockCarService.Setup(s => s.GetCarsByHubAddressAsync(hubAddress)).ReturnsAsync(expectedCars);

            // Act
            var result = await _controller.GetCarDetailsByHubAddress(hubAddress);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(expectedCars);
        }

        [Fact]
        public async Task GetAvailableCars_ReturnsOkResult_WhenDatesAreValid()
        {
            // Arrange
            int hubId = 1;
            string startDate = "2023-10-01";
            string endDate = "2023-10-10";
            long? carTypeId = 1;

            var expectedCars = new List<CarMaster>
            {
                new CarMaster { CarId = 1, CarName = "Toyota Camry" }
            };

            _mockCarService.Setup(s => s.GetAvailableCarsAsync(hubId, It.IsAny<DateTime>(), It.IsAny<DateTime>(), carTypeId))
                           .ReturnsAsync(expectedCars);

            // Act
            var result = await _controller.GetAvailableCars(hubId, startDate, endDate, carTypeId);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(expectedCars);
        }

        [Fact]
        public async Task GetAvailableCars_ReturnsBadRequest_WhenDatesAreInvalid()
        {
            // Arrange
            int hubId = 1;
            string startDate = "invalid-date";
            string endDate = "2023-10-10";

            // Act
            var result = await _controller.GetAvailableCars(hubId, startDate, endDate, null);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UploadFile_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("test.xlsx");
            _mockExcelService.Setup(s => s.SaveCarsAsync(fileMock.Object)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UploadFile(fileMock.Object);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task UploadFile_ReturnsExpectationFailed_WhenExceptionOccurs()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns("test.xlsx");
            _mockExcelService.Setup(s => s.SaveCarsAsync(fileMock.Object)).ThrowsAsync(new Exception("Upload failed"));

            // Act
            var result = await _controller.UploadFile(fileMock.Object);

            // Assert
            var statusCodeResult = result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(StatusCodes.Status417ExpectationFailed);
        }
    }
}
