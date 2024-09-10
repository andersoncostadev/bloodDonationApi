using Application.Dtos;
using Application.Interfaces;
using Application.Queries.Address.GetAll;
using Moq;

namespace Test.Unit.Application.Commands.v1.Address
{
    [TestFixture]
    public class GetAllAddressQueryHandlerTest
    {
        private Mock<IAddressUseCases> _addressUseCases;
        private GetAllAddressQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _addressUseCases = new Mock<IAddressUseCases>();
            _handler = new GetAllAddressQueryHandler(_addressUseCases.Object);
        }

        [Test]
        public void Handle_ShouldReturnAddressesSuccessfully_WhenValid()
        {
            //Arrange
            var query = new GetAllAddressQuery(1, 10);

            var addresses = new List<AddressDto>
            {
                new() { Id = Guid.NewGuid(), Street = "Main St", City = "New York", State = "NY", ZipCode = "10001" },
                new() { Id = Guid.NewGuid(), Street = "Main St", City = "New York", State = "NY", ZipCode = "10001" }
            };

            _addressUseCases.Setup(x => x.GetAddressesAsync(query.PageNumber, query.PageSize)).ReturnsAsync(addresses);

            //Act
            var response = _handler.Handle(query, CancellationToken.None).Result;

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response.Addresses!.Count(), Is.EqualTo(addresses.Count));
                Assert.That(response.TotalPages, Is.EqualTo(1));
                Assert.That(response.PageNumber, Is.EqualTo(query.PageNumber));
            });
        }

        [Test]
        public void Handle_ShouldThrowApplicationException_WhenAddressesAreNotRetrieved()
        {
            //Arrange
            var query = new GetAllAddressQuery(1, 10);

            _addressUseCases.Setup(x => x.GetAddressesAsync(query.PageNumber, query.PageSize)).ReturnsAsync((List<AddressDto>)null);

            //Act & Assert
            var exception = Assert.ThrowsAsync<ApplicationException>(async () => await _handler.Handle(query, CancellationToken.None));
            Assert.That(exception.Message, Does.Contain("Failed to get addresses"));
        }
    }
}
