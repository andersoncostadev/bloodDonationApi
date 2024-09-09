using Application.Commands.v1.Donor.Update;
using Application.Dtos;
using Application.Interfaces;
using FluentValidation;
using Moq;

namespace Test.Unit.Application.Commands.v1.Donor
{
    [TestFixture]
    public class UpdateDonorCommandHandlerTest
    {
        private Mock<IDonorUseCases> _mockDonorUseCases;
        private Mock<IAddressUseCases> _mockAddressUseCases;
        private Mock<IValidator<UpdateDonorCommand>> _mockValidator;
        private UpdateDonorCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockDonorUseCases = new Mock<IDonorUseCases>();
            _mockAddressUseCases = new Mock<IAddressUseCases>();
            _mockValidator = new Mock<IValidator<UpdateDonorCommand>>();
            _handler = new UpdateDonorCommandHandler(_mockDonorUseCases.Object, _mockAddressUseCases.Object, _mockValidator.Object);
        }

        [Test]
        public void TaskHandle_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var command = new UpdateDonorCommand();
            var validationFailure = new FluentValidation.Results.ValidationFailure("FullName", "FullName is required");
            var validationResult = new FluentValidation.Results.ValidationResult(new[] { validationFailure });

            _mockValidator.Setup(x => x.ValidateAsync(command, default)).ReturnsAsync(validationResult);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(exception.Message, Does.Contain("FullName is required"));
        }

        [Test]
        public void TaskHandle_ShouldThrowException_WhenDonorNotFound()
        {
            // Arrange
            var command = new UpdateDonorCommand();
            var donor = new DonorDto { Id = command.Id };

            _mockValidator.Setup(x => x.ValidateAsync(command, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockDonorUseCases.Setup(x => x.GetDonorByIdAsync(command.Id)).ReturnsAsync((DonorDto)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(exception.Message, Does.Contain("Donor not found"));
        }

        [Test]
        public void TaskHandle_ShouldUpdateDonorSuccessfully_WhenValidCommand()
        {
            // Arrange
            var command = new UpdateDonorCommand
            {
                Id = Guid.NewGuid(),
                FullName = "John Doe",
                Email = "johndoe@example.com",
                Weight = 75.5
            };

            var donor = new DonorDto
            {
                Id = command.Id,
                FullName = "Jane Doe",
                Email = "janedoe@example.com",
                Weight = 70
            };

            _mockValidator.Setup(x => x.ValidateAsync(command, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockDonorUseCases.Setup(x => x.GetDonorByIdAsync(command.Id)).ReturnsAsync(donor);

            _mockDonorUseCases.Setup(x => x.UpdateDonorAsync(It.IsAny<DonorDto>())).ReturnsAsync(donor);

            // Act
            var result = _handler.Handle(command, CancellationToken.None).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(donor.Id));
                Assert.That(result.FullName, Is.EqualTo(command.FullName));
                Assert.That(result.Email, Is.EqualTo(command.Email));
                Assert.That(result.BirthDate, Is.EqualTo(donor.BirthDate));
            });
        }

        [Test]
        public void Handle_ShouldUpdateDonorWithAddressSuccessfully()
        {
            // Arrange
            var command = new UpdateDonorCommand
            {
                Id = Guid.NewGuid(),
                FullName = "John Doe",
                Email = "johndoe@example.com",
                Weight = 75.5,
                Address = new AddressDto
                {
                    Street = "Rua Nova",
                    Number = "123",
                    Neighborhood = "Centro",
                    City = "São Paulo",
                    State = "SP",
                    ZipCode = "12345-000"
                }
            };

            var donor = new DonorDto
            {
                Id = command.Id,
                FullName = "Jane Doe",
                Email = "janedoe@example.com",
                Weight = 70,
                Address = new AddressDto
                {
                    Street = "Rua Velha",
                    Number = "456",
                    Neighborhood = "Centro",
                    City = "São Paulo",
                    State = "SP",
                    ZipCode = "12345-000"
                }
            };

            _mockValidator.Setup(x => x.ValidateAsync(command, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mockDonorUseCases.Setup(x => x.GetDonorByIdAsync(command.Id)).ReturnsAsync(donor);

            _mockDonorUseCases.Setup(x => x.UpdateDonorAsync(It.IsAny<DonorDto>())).ReturnsAsync(donor);

            // Act
            var result = _handler.Handle(command, CancellationToken.None).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(donor.Id));
                Assert.That(result.FullName, Is.EqualTo(command.FullName));
                Assert.That(result.Email, Is.EqualTo(command.Email));
                Assert.That(result.BirthDate, Is.EqualTo(donor.BirthDate));
                Assert.That(result.Address, Is.Not.Null);
                Assert.That(result.Address!.Street, Is.EqualTo(command.Address.Street));
                Assert.That(result.Address.Number, Is.EqualTo(command.Address.Number));
                Assert.That(result.Address.Neighborhood, Is.EqualTo(command.Address.Neighborhood));
                Assert.That(result.Address.City, Is.EqualTo(command.Address.City));
                Assert.That(result.Address.State, Is.EqualTo(command.Address.State));
                Assert.That(result.Address.ZipCode, Is.EqualTo(command.Address.ZipCode));
            });
        }
    } 
}