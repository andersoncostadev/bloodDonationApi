using Application.Commands.v1.StockBlood;
using Application.Dtos;
using Application.Interfaces;
using Moq;

namespace Test.Unit.Application.Commands.v1.StockBlood
{
    public class UpdateStockBloodCommandHandlerTest
    {
        private Mock<IStockBloodUseCases> _stockBloodUseCases;
        private UpdateStockBloodCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _stockBloodUseCases = new Mock<IStockBloodUseCases>();
            _handler = new UpdateStockBloodCommandHandler(_stockBloodUseCases.Object);
        }

        [Test]
        public void Handle_ShouldUpdateStockBlood_WhenValidCommand()
        {
            //Arrange
            var command = new UpdateStockBloodCommand
            {
                BloodType = "A",
                RhFactor = "+",
                QuantityML = 450
            };

            var stockBlood = new StockBloodDto
            {
                BloodType = "A",
                RhFactor = "+",
                QuantityML = 450
            };

            _stockBloodUseCases.Setup(x => x.GetByBloodTypeAndRhFactorAsync(command.BloodType, command.RhFactor)).ReturnsAsync(stockBlood);
            _stockBloodUseCases.Setup(x => x.AdjustStockQuantityAsync(stockBlood)).ReturnsAsync(stockBlood);

            //Act
            var response = _handler.Handle(command, CancellationToken.None).Result;

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response.BloodType, Is.EqualTo(stockBlood.BloodType));
                Assert.That(response.RhFactor, Is.EqualTo(stockBlood.RhFactor));
                Assert.That(response.QuantityML, Is.EqualTo(stockBlood.QuantityML));
            });
        }

        [Test]
        public void Handle_ShouldThrowApplicationException_WhenStockBloodIsNotUpdated()
        {
            //Arrange
            var command = new UpdateStockBloodCommand
            {
                BloodType = "A",
                RhFactor = "+",
                QuantityML = 450
            };

            _stockBloodUseCases.Setup(x => x.GetByBloodTypeAndRhFactorAsync(command.BloodType, command.RhFactor)).ReturnsAsync((StockBloodDto)null);

            //Act & Assert
            var exception = Assert.ThrowsAsync<ApplicationException>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(exception.Message, Does.Contain($"No stock found for BloodType {command.BloodType} and RhFactor {command.RhFactor}"));
        }
    }
}
