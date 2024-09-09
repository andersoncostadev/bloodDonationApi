using Application.Commands.v1.Donor.Delete;
using Application.Dtos;
using Application.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Test.Unit.Application.Commands.v1.Donor
{
    [TestFixture]
    public class DeleteDonorCommandHandlerTest
    {
        public Mock<IDonorUseCases> _donorUseCases;
        public Mock<IValidator<DeleteDonorCommand>> _validator;
        public DeleteDonorCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _donorUseCases = new Mock<IDonorUseCases>();
            _validator = new Mock<IValidator<DeleteDonorCommand>>();
            _handler = new DeleteDonorCommandHandler(_donorUseCases.Object, _validator.Object);
        }

        [Test]
        public void HandleShouldThrowValidationExceptionWhenValidationFails()
        {
            // Arrange
            var command = new DeleteDonorCommand(Guid.NewGuid());
            var validationFailure = new ValidationFailure("Id", "Invalid Id");
            var validationResult = new ValidationResult(new List<ValidationFailure> { validationFailure });

            _validator.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(exception.Message, Does.Contain("Invalid Id"));
        }

        [Test]
        public void Handle_ShouldThrowApplicationException_WhenDonorNotFound()
        {
            // Arrange
            var command = new DeleteDonorCommand(Guid.NewGuid());

            _validator.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            _donorUseCases.Setup(x => x.GetDonorByIdAsync(command.Id)).ReturnsAsync((DonorDto)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ApplicationException>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(exception.Message, Does.Contain("Donor not found"));
        }

        [Test]
        public void Handle_ShouldThrowApplicationException_WhenDeletionFails()
        {
            //Arrange
            var command = new DeleteDonorCommand(Guid.NewGuid());
            var donor = new DonorDto { Id = command.Id };

            _validator.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            _donorUseCases.Setup(x => x.GetDonorByIdAsync(command.Id)).ReturnsAsync(donor);

            _donorUseCases.Setup(x => x.DeleteDonorAsync(donor.Id)).ReturnsAsync(false);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ApplicationException>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(exception.Message, Does.Contain("Failed to delete donor"));
        }

        [Test]
        public void Handle_ShouldDeleteDonorSuccessfully_WhenValidCommand()
        {
            // Arrange
            var command = new DeleteDonorCommand(Guid.NewGuid());
            var donor = new DonorDto { Id = command.Id };

            _validator.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            _donorUseCases.Setup(x => x.GetDonorByIdAsync(command.Id)).ReturnsAsync(donor);

            _donorUseCases.Setup(x => x.DeleteDonorAsync(donor.Id)).ReturnsAsync(true);

            // Act
            var response = _handler.Handle(command, CancellationToken.None).Result;

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<DeleteDonorCommandResponse>());
        }
    }
}
