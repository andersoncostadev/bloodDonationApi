using Application.Dtos;
using Application.Exceptions;
using Application.Interfaces;
using Application.Queries.Donation.GetDonationsReport;
using AutoMapper;
using Moq;

namespace Test.Unit.Application.Commands.v1.Donation
{
    [TestFixture]
    public class GetDonationsReportQueryHandlerTest
    {
        private Mock<IDonationUseCases> _donationUseCases;
        private Mock<IDonorUseCases> _donorUseCases;
        private Mock<IMapper> _mockMapper;
        private GetDonationsReportQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _donationUseCases = new Mock<IDonationUseCases>();
            _donorUseCases = new Mock<IDonorUseCases>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetDonationsReportQueryHandler(_donationUseCases.Object, _donorUseCases.Object, _mockMapper.Object);
        }

        [Test]
        public void Handle_ShouldReturnDonationsReportSuccessfully_WhenValid()
        {
            //Arrange
            var query = new GetDonationsReportQuery(1, 10);

            var donations = new List<DonationDto>
            {
                new() { DonorId = Guid.NewGuid(), DonationDate = DateTime.UtcNow, QuantityML = 450 },
                new() { DonorId = Guid.NewGuid(), DonationDate = DateTime.UtcNow, QuantityML = 450 }
            };

            var donor = new DonorDto
            {
                Id = Guid.NewGuid(),
                FullName = "John Doe",
                BloodType = "A",
                RhFactor = "+"
            };

            _donationUseCases.Setup(x => x.GetDonationsReportAsync(query.PageNumber, query.PageSize)).ReturnsAsync(donations);

            _donorUseCases.Setup(x => x.GetDonorByIdAsync(It.IsAny<Guid>())).ReturnsAsync(donor);

            _mockMapper.Setup(m => m.Map<DonationReportDto>(It.IsAny<DonationDto>()))
               .Returns((DonationDto src) => new DonationReportDto
               {
                   DonorId = src.DonorId,
                   DonationDate = src.DonationDate,
                   QuantityML = src.QuantityML
               });

            //Act
            var response = _handler.Handle(query, CancellationToken.None).Result;

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response.Donations!, Has.Count.EqualTo(donations.Count));
                Assert.That(response.TotalPages, Is.EqualTo(1));
                Assert.That(response.PageNumber, Is.EqualTo(query.PageNumber));
                Assert.That(response.PageSize, Is.EqualTo(10));
            });
        }

        [Test]
        public void Handle_ShouldThrowApplicationException_WhenDonationsAreNotRetrieved()
        {
            //Arrange
            var query = new GetDonationsReportQuery(1, 10);

            _donationUseCases.Setup(x => x.GetDonationsReportAsync(query.PageNumber, query.PageSize)).ReturnsAsync((List<DonationDto>)null);

            //Act & Assert
            var exception = Assert.ThrowsAsync<ApplicationException>(async () => await _handler.Handle(query, CancellationToken.None));
            Assert.That(exception.Message, Does.Contain("Failed to get donations report"));
        }

        [Test]
        public void Handle_ShouldThrowDonorNotFoundException_WhenDonorIsNotFound()
        {
            // Arrange
            var query = new GetDonationsReportQuery(1, 10);
            var donations = new List<DonationDto>
            {
                new DonationDto { Id = Guid.NewGuid(), DonorId = Guid.NewGuid(), QuantityML = 450, DonationDate = DateTime.UtcNow }
            };

            _donationUseCases.Setup(x => x.GetDonationsReportAsync(query.PageNumber, query.PageSize))
                .ReturnsAsync(donations);

            _donorUseCases.Setup(x => x.GetDonorByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((DonorDto)null); // Simula doador não encontrado

            // Act & Assert
            var exception = Assert.ThrowsAsync<DonorNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
