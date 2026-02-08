using System.Collections.Generic;
using System.Threading.Tasks;
using FleetManagementSystem.Api.Controllers;
using FleetManagementSystem.Api.Models;
using FleetManagementSystem.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;

namespace FleetManagementSystem.Tests
{
    public class CarTypeMasterControllerTests
    {
        private readonly Mock<ICarTypeMasterService> _mockService;
        private readonly CarTypeMasterController _controller;

        public CarTypeMasterControllerTests()
        {
            _mockService = new Mock<ICarTypeMasterService>();
            _controller = new CarTypeMasterController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllCarTypes_ReturnsOkResult_WithListOfCarTypes()
        {
            // Arrange
            var expectedCarTypes = new List<CarTypeMaster>
            {
                new CarTypeMaster { CarTypeId = 1, CarTypeName = "SUV" },
                new CarTypeMaster { CarTypeId = 2, CarTypeName = "Sedan" }
            };
            _mockService.Setup(s => s.GetAllCarTypesAsync()).ReturnsAsync(expectedCarTypes);

            // Act
            var result = await _controller.GetAllCarTypes();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var actualCarTypes = okResult.Value.Should().BeAssignableTo<List<CarTypeMaster>>().Subject;
            actualCarTypes.Should().HaveCount(2);
            actualCarTypes.Should().BeEquivalentTo(expectedCarTypes);
        }
    }
}
