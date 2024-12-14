using Microsoft.Extensions.Logging;
using Moq;
using TariffComparison.Factory;
using TariffComparison.Models;
using TariffComparison.Providers;
using TariffComparison.Repository;
using TariffComparison.Services.TariffCalculators;

namespace TariffComparison.Test.Repository
{
    public class TariffComparisonRepositoryTest
    {
        private readonly Mock<ITariffProvider> _providerMock;
        private readonly Mock<ITariffCalculatorFactory> _calculatorFactoryMock;
        private readonly Mock<ILogger<TariffComparisonRepository>> _loggerMock;
        private readonly TariffComparisonRepository _repository;

        public TariffComparisonRepositoryTest()
        {
            _providerMock = new Mock<ITariffProvider>();
            _calculatorFactoryMock = new Mock<ITariffCalculatorFactory>();
            _loggerMock = new Mock<ILogger<TariffComparisonRepository>>();

            _repository = new TariffComparisonRepository(_providerMock.Object, _calculatorFactoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CompareTariffsAsync_ReturnsSortedResults_ByAnnualCost()
        {
            // Arrange
            var tariffs = new List<TariffModel>
            {
                new TariffModel { Name = "Basic", Type = 1 },
                new TariffModel { Name = "Packaged", Type = 2 }
            };

            _providerMock.Setup(p => p.GetTariffsAsync()).ReturnsAsync(tariffs);

            _calculatorFactoryMock.Setup(f => f.GetCalculator(It.Is<TariffModel>(t => t.Type == 1)))
                .Returns(Mock.Of<ITariffCalculator>(c => c.CalculateAnnualCost(100) == 1200));

            _calculatorFactoryMock.Setup(f => f.GetCalculator(It.Is<TariffModel>(t => t.Type == 2)))
                .Returns(Mock.Of<ITariffCalculator>(c => c.CalculateAnnualCost(100) == 1000));

            // Act
            var result = await _repository.CompareTariffsAsync(100);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Packaged", result.First().TariffName);
            Assert.Equal("Basic", result.Last().TariffName);
        }
    }
}
