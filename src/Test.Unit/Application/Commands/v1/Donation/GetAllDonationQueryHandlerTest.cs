using Application.Dtos;
using Application.Interfaces;
using Application.Queries.Donation.GetAll;
using Moq;

namespace Test.Unit.Application.Commands.v1.Donation
{
    [TestFixture]
    public class GetAllDonationQueryHandlerTest
    {
        private Mock<IDonationUseCases> _donationUseCases;
        private GetAllDonationQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _donationUseCases = new Mock<IDonationUseCases>();
            _handler = new GetAllDonationQueryHandler(_donationUseCases.Object);
        }

        [Test]
        public void Handle_ShouldReturnDonationsSuccessfully_WhenValid()
        {
            //Arrange
            var query = new GetAllDonationQuery(1, 10);

            var donations = new List<DonationDto>
            {
                new() { DonorId = Guid.NewGuid(), DonationDate = DateTime.UtcNow, QuantityML = 450 },
                new() { DonorId = Guid.NewGuid(), DonationDate = DateTime.UtcNow, QuantityML = 450 }
            };

            _donationUseCases.Setup(x => x.GetDonationsAsync(query.PageNumber, query.PageSize)).ReturnsAsync(donations);

            //Act
            var response = _handler.Handle(query, CancellationToken.None).Result;

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response.Donations!.Count(), Is.EqualTo(donations.Count));
                Assert.That(response.TotalPages, Is.EqualTo(1));
                Assert.That(response.PageNumber, Is.EqualTo(query.PageNumber));
            });
        }

        [Test]
        public void Handle_ShouldThrowApplicationException_WhenDonationsAreNotRetrieved()
        {
            //Arrange
            var query = new GetAllDonationQuery(1, 10);

            _donationUseCases.Setup(x => x.GetDonationsAsync(query.PageNumber, query.PageSize)).ReturnsAsync((List<DonationDto>)null);

            //Act & Assert
            var exception = Assert.ThrowsAsync<ApplicationException>(async () => await _handler.Handle(query, CancellationToken.None));
            Assert.That(exception.Message, Does.Contain("Failed to get donations"));
        }
    }
}
