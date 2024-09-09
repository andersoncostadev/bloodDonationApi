using Application.Dtos;
using Application.Interfaces;
using Application.Queries.Donor.GetById;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Test.Unit.Application.Commands.v1.Donor
{
    [TestFixture]
    public class GetByIdDonorQueryHandlerTest
    {
        private Mock<IDonorUseCases> _mockDonorUseCases;
        private Mock<IValidator<GetByIdDonorQuery>> _mockValidator;
        private GetByIdDonorQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockDonorUseCases = new Mock<IDonorUseCases>();
            _mockValidator = new Mock<IValidator<GetByIdDonorQuery>>();
            _handler = new GetByIdDonorQueryHandler(_mockDonorUseCases.Object, _mockValidator.Object);
        }

        [Test]
        public void Handle_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var query = new GetByIdDonorQuery(Guid.NewGuid());
            var validationFailure = new ValidationFailure("Id", "Invalid ID format");
            var validationResult = new ValidationResult(new[] { validationFailure });

            _mockValidator.Setup(v => v.Validate(query))
                .Returns(validationResult);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.That(exception.Message, Does.Contain("Invalid ID format"));
        }

        [Test]
        public void Handle_ShouldReturnDonor_WhenDonorExists()
        {
            // Arrange
            var query = new GetByIdDonorQuery(Guid.NewGuid());

            var donor = new DonorDto
            {
                Id = query.Id,
                FullName = "John Doe",
                Email = "john.doe@example.com",
                BirthDate = new DateTime(1985, 5, 15),
                Gender = Domain.Enums.v1.DonorGender.MALE,
                Weight = 75.5,
                BloodType = "O",
                RhFactor = "+",
                Address = new AddressDto { Street = "Rua Brasil", Number = "100" }
            };

            _mockValidator.Setup(v => v.Validate(query))
                .Returns(new ValidationResult());

            _mockDonorUseCases.Setup(x => x.GetDonorByIdAsync(query.Id))
                .ReturnsAsync(donor);

            // Act
            var response = _handler.Handle(query, CancellationToken.None).Result;


            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response.Id, Is.EqualTo(donor.Id));
                Assert.That(response.FullName, Is.EqualTo(donor.FullName));
                Assert.That(response.Email, Is.EqualTo(donor.Email));
                Assert.That(response.BirthDate, Is.EqualTo(donor.BirthDate));
                Assert.That(response.Gender, Is.EqualTo(donor.Gender));
                Assert.That(response.Weight, Is.EqualTo(donor.Weight));
                Assert.That(response.BloodType, Is.EqualTo(donor.BloodType));
                Assert.That(response.RhFactor, Is.EqualTo(donor.RhFactor));
                Assert.That(response.Address, Is.Not.Null);
            });
        }
    }
}
