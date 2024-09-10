using Application.Dtos;
using Application.Interfaces;
using Application.Queries.StockBlood.GetStockBloodReport;
using Moq;

namespace Test.Unit.Application.Commands.v1.StockBlood
{
    public class GetStockBloodReportQueryHandlerTest
    {
        private Mock<IStockBloodUseCases> _stockBloodUseCases;
        private GetStockBloodReportQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _stockBloodUseCases = new Mock<IStockBloodUseCases>();
            _handler = new GetStockBloodReportQueryHandler(_stockBloodUseCases.Object);
        }

        [Test]
        public void Handle_ShouldReturnStockBloodReport_WhenValidQuery()
        {
            //Arrange
            var query = new GetStockBloodReportQuery(1, 10);

            var stockBlood = new List<StockBloodDto>
            {
                new() { BloodType = "A", RhFactor = "+", QuantityML = 450 },
                new() { BloodType = "B", RhFactor = "-", QuantityML = 450 }
            };

            _stockBloodUseCases.Setup(x => x.GetBloodStockReportAsync(query.PageNumber, query.PageSize)).ReturnsAsync(stockBlood);

            //Act
            var response = _handler.Handle(query, CancellationToken.None).Result;

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.StockBlood!.Count(), Is.EqualTo(stockBlood.Count));
                Assert.That(response.TotalPages, Is.EqualTo(1));
                Assert.That(response.PageNumber, Is.EqualTo(query.PageNumber));
                Assert.That(response.PageSize, Is.EqualTo(query.PageSize));
            });
        }

        [Test]
        public void Handle_ShouldThrowApplicationException_WhenStockBloodReportIsNotRetrieved()
        {
            //Arrange
            var query = new GetStockBloodReportQuery(1, 10);

            _stockBloodUseCases.Setup(x => x.GetBloodStockReportAsync(query.PageNumber, query.PageSize)).ReturnsAsync((List<StockBloodDto>)null);

            //Act & Assert
            var exception = Assert.ThrowsAsync<ApplicationException>(async () => await _handler.Handle(query, CancellationToken.None));
            Assert.That(exception.Message, Does.Contain("Failed to get stock blood report"));
        }
    }
}
