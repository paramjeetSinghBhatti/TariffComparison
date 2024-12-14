using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TariffComparison.Controllers;
using TariffComparison.Models;
using TariffComparison.Repository;

namespace TariffComparison.Test.Controllers
{
    public class TariffComparisonControllerTests
    {
        private readonly Mock<ITariffComparisonRepository> _repositoryMock;
        private readonly Mock<ILogger<TariffComparisonController>> _loggerMock;
        private readonly TariffComparisonController _controller;

        public TariffComparisonControllerTests()
        {
            _repositoryMock = new Mock<ITariffComparisonRepository>();
            _loggerMock = new Mock<ILogger<TariffComparisonController>>();
            _controller = new TariffComparisonController(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CompareTariffs_ReturnsBadRequest_WhenConsumptionIsZeroOrNegative()
        {
            // Arrange
            int invalidConsumption = 0;

            // Act
            var result = await _controller.CompareTariffs(invalidConsumption);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Consumption must be greater than 0.", badRequestResult.Value);
        }

        [Fact]
        public async Task CompareTariffs_ReturnsInternalServerError_WhenResultsAreNull()
        {
            // Arrange
            int consumption = 100;
            _repositoryMock.Setup(repo => repo.CompareTariffsAsync(consumption))
                .ReturnsAsync((IEnumerable<TariffComparisonResult>)null);

            // Act
            var result = await _controller.CompareTariffs(consumption);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task CompareTariffs_ReturnsInternalServerError_WhenResultsAreEmpty()
        {
            // Arrange
            int consumption = 100;
            _repositoryMock.Setup(repo => repo.CompareTariffsAsync(consumption))
                .ReturnsAsync(new List<TariffComparisonResult>());

            // Act
            var result = await _controller.CompareTariffs(consumption);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task CompareTariffs_ReturnsOk_WhenResultsAreValid()
        {
            // Arrange
            int consumption = 100;
            var expectedResults = new List<TariffComparisonResult>
            {
                new TariffComparisonResult { TariffName = "Basic", AnnualCost = 100.0M },
                new TariffComparisonResult { TariffName = "Packaged", AnnualCost = 200.0M }
            };

            _repositoryMock.Setup(repo => repo.CompareTariffsAsync(consumption))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _controller.CompareTariffs(consumption);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResults = Assert.IsAssignableFrom<IEnumerable<TariffComparisonResult>>(okResult.Value);
            Assert.Equal(expectedResults.Count, actualResults.Count());
        }

        [Fact]
        public async Task CompareTariffs_ReturnsInternalServerError_OnException()
        {
            // Arrange
            int consumption = 100;
            _repositoryMock.Setup(repo => repo.CompareTariffsAsync(consumption))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.CompareTariffs(consumption);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }
    }
}
