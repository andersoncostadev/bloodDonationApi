using Application.Commands.v1.Donation.Create;
using Application.Dtos;
using Application.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Test.Unit.Application.Commands.v1.Donation
{
    [TestFixture]
    public class CreateDonationCommandHandlerTest
    {
        private Mock<IDonationUseCases> _donationUseCases;
        private Mock<IDonorUseCases> _donorUseCases;
        private Mock<IStockBloodUseCases> _stockBloodUseCases;
        private Mock<IValidator<CreateDonationCommand>> _validator;
        private CreateDonationCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _donationUseCases = new Mock<IDonationUseCases>();
            _donorUseCases = new Mock<IDonorUseCases>();
            _stockBloodUseCases = new Mock<IStockBloodUseCases>();
            _validator = new Mock<IValidator<CreateDonationCommand>>();
            _handler = new CreateDonationCommandHandler(_donationUseCases.Object, _donorUseCases.Object, _stockBloodUseCases.Object, _validator.Object);
        }

        [Test]
        public void HandleShouldThrowValidationExceptionWhenValidationFails()
        {
            // Arrange
            var command = new CreateDonationCommand
            {
                DonorId = Guid.NewGuid(),
                DonationDate = DateTime.UtcNow,
                QuantityML = 450
            };
            var validationFailure = new ValidationFailure("DonationDate", "Quantity must be valid");
            var validationResult = new ValidationResult(new List<ValidationFailure> { validationFailure });

            _validator.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(exception.Message, Does.Contain("Quantity must be valid"));
        }

        [Test]
        public void Handle_ShouldThrowApplicationException_WhenDonorNotFound()
        {
            // Arrange
            var command = new CreateDonationCommand
            {
                DonorId = Guid.NewGuid(),
                DonationDate = DateTime.UtcNow,
                QuantityML = 450
            };

            _validator.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            _donorUseCases.Setup(x => x.GetDonorByIdAsync(command.DonorId)).ReturnsAsync((DonorDto)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ApplicationException>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(exception.Message, Does.Contain("Donor not found"));
        }

        [Test]
        public void Handle_ShouldThrowApplicationException_WhenDonationCreationFails()
        {
            // Arrange
            var command = new CreateDonationCommand
            {
                DonorId = Guid.NewGuid(),
                DonationDate = DateTime.UtcNow,
                QuantityML = 450
            };
            var donor = new DonorDto
            {
                Id = command.DonorId,
                BloodType = "O",
                RhFactor = "+"
            };

            _validator.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            _donorUseCases.Setup(x => x.GetDonorByIdAsync(command.DonorId)).ReturnsAsync(donor);

            _donationUseCases.Setup(u => u.CreateDonationAsync(It.IsAny<DonationDto>()))
                .ReturnsAsync((DonationDto)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ApplicationException>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(exception.Message, Does.Contain("Failed to create donation"));
        }

        [Test]
        public void Handle_ShouldCreateDonationSuccessfully_WhenValid()
        {
            //Arrange
            var command = new CreateDonationCommand
            {
                DonorId = Guid.NewGuid(),
                DonationDate = DateTime.UtcNow,
                QuantityML = 450
            };

            var donor = new DonorDto
            {
                Id = command.DonorId,
                BloodType = "O",
                RhFactor = "+"
            };

            var createdDonation = new DonationDto
            {
                Id = Guid.NewGuid(),
                DonorId = command.DonorId,
                DonationDate = command.DonationDate,
                QuantityML = command.QuantityML
            };

            var stockBlood = new StockBloodDto
            {
                Id = Guid.NewGuid(),
                BloodType = donor.BloodType,
                RhFactor = donor.RhFactor,
                QuantityML = createdDonation.QuantityML
            };

            _validator.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            _donorUseCases.Setup(x => x.GetDonorByIdAsync(command.DonorId)).ReturnsAsync(donor);

            _donationUseCases.Setup(u => u.CreateDonationAsync(It.IsAny<DonationDto>()))
                .ReturnsAsync(createdDonation);

            _stockBloodUseCases.Setup(u => u.UpdateStockBloodAsync(It.IsAny<StockBloodDto>()))
                .ReturnsAsync(stockBlood);


            // Act
            var result = _handler.Handle(command, CancellationToken.None).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(createdDonation.Id));
                Assert.That(result.DonorId, Is.EqualTo(createdDonation.DonorId));
                Assert.That(result.DonationDate, Is.EqualTo(createdDonation.DonationDate));
                Assert.That(result.QuantityML, Is.EqualTo(createdDonation.QuantityML));
            });
        }
    }
}
