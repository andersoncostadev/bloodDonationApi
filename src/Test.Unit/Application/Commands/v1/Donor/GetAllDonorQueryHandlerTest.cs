using Application.Dtos;
using Application.Interfaces;
using Application.Queries.Donor.GetAll;
using Moq;

namespace Test.Unit.Application.Commands.v1.Donor
{
    [TestFixture]
    public class GetAllDonorQueryHandlerTest
    {
        private Mock<IDonorUseCases> _mockDonorUseCases;
        private GetAllDonorQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockDonorUseCases = new Mock<IDonorUseCases>();
            _handler = new GetAllDonorQueryHandler(_mockDonorUseCases.Object);
        }

        [Test]
        public void Handle_ShouldReturnDonors_WhenCalledWithValidParameters()
        {
            //Arrange
            var query = new GetAllDonorQuery(1, 10);

            var donors = new List<DonorDto>
            {
                new() { FullName = "John Doe", Email = "john.doe@example.com" },
                new() { FullName = "Jane Doe", Email = "jane.doe@example.com" }
            };

            _mockDonorUseCases.Setup(x => x.GetDonorsAsync(query.PageNumber, query.PageSize)).ReturnsAsync(donors);

            //Act
            var response = _handler.Handle(query, CancellationToken.None).Result;

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response.Donors!.Count(), Is.EqualTo(donors.Count));
                Assert.That(response.TotalPages, Is.EqualTo(1));
                Assert.That(response.PageNumber, Is.EqualTo(query.PageNumber));
            });
        }

        [Test]
        public void Handle_ShouldThrowApplicationException_WhenDonorsIsNull()
        {
            //Arrange
            var query = new GetAllDonorQuery(1, 10);

            _mockDonorUseCases.Setup(x => x.GetDonorsAsync(query.PageNumber, query.PageSize)).ReturnsAsync((List<DonorDto>)null);

            //Act & Assert
            var exception = Assert.ThrowsAsync<ApplicationException>(async () => await _handler.Handle(query, CancellationToken.None));
            Assert.That(exception.Message, Does.Contain("Failed to get donors"));
        }

        [Test]
        public void Handle_ShouldReturnCorrectTotalPages_WhenDonorsArePaginated()
        {
            var query = new GetAllDonorQuery(1, 2);

            var donors = new List<DonorDto>
            {
                new() { FullName = "John Doe", Email = "john.doe@example.com" },
                new() { FullName = "Jane Doe", Email = "jane.doe@example.com" },
                new() { FullName = "Jim Doe", Email = "jim.doe@example.com" }
            };

            _mockDonorUseCases.Setup(x => x.GetDonorsAsync(query.PageNumber, query.PageSize)).ReturnsAsync(donors);

            //Act
            var response = _handler.Handle(query, CancellationToken.None).Result;

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.TotalPages, Is.EqualTo(2));
        }
    }
}
