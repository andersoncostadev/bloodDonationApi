using Application.Dtos;
using Application.Interfaces;
using Application.Queries.Donor.GetFullName;
using FluentValidation;
using Moq;

namespace Test.Unit.Application.Commands.v1.Donor
{
    [TestFixture]
    public class GetByFullNameQueryHandlerTest
    {
        private Mock<IDonorUseCases> _mockUseCases;
        private Mock<IValidator<GetByFullNameQuery>> _validator;
        private GetByFullNameQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockUseCases = new Mock<IDonorUseCases>();
            _validator = new Mock<IValidator<GetByFullNameQuery>>();
            _handler = new GetByFullNameQueryHandler(_mockUseCases.Object, _validator.Object);
        }

        [Test]
        public void TaskHandle_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var query = new GetByFullNameQuery("John Doe");
            var validationFailure = new FluentValidation.Results.ValidationFailure("FullName", "FullName is required");
            var validationResult = new FluentValidation.Results.ValidationResult(new[] { validationFailure });

            _validator.Setup(x => x.ValidateAsync(query, default)).ReturnsAsync(validationResult);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(query, default));
            Assert.That(exception.Message, Does.Contain("FullName is required"));
        }

        [Test]
        public void Handle_ShouldThrowApplicationException_WhenDonorNotFound()
        {
            // Arrange
            var query = new GetByFullNameQuery("John Doe");

            _validator.Setup(x => x.ValidateAsync(query, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockUseCases.Setup(x => x.GetDonorFullNameAsync(query.FullName)).ReturnsAsync((DonorDto)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ApplicationException>(async () => await _handler.Handle(query, default));
            Assert.That(exception.Message, Does.Contain("Donor not found"));
        }

        [Test]
        public void Handle_ShouldReturnDonor_WhenDonorExists()
        {             // Arrange
            var query = new GetByFullNameQuery("John Doe");

            var donor = new DonorDto
            {
                Id = Guid.NewGuid(),
                FullName = "John Doe",
                Email = "john.doe@example.com",
                BirthDate = new DateTime(1985, 5, 15),
                Gender = Domain.Enums.v1.DonorGender.MALE,
                Weight = 75.5,
                BloodType = "O",
                RhFactor = "+",
                Address = new AddressDto { Street = "Rua Brasil", Number = "100" }
            };

            _validator.Setup(x => x.ValidateAsync(query, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockUseCases.Setup(x => x.GetDonorFullNameAsync(query.FullName)).ReturnsAsync(donor);

            // Act
            var response = _handler.Handle(query, default).Result;

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
