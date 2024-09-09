using Application.Commands.v1.Donor.Create;
using Application.Dtos;
using Application.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Test.Unit.Application.Commands.v1.Donor
{
    [TestFixture]
    public class CreateDonorCommandHandlerTest
    {
        private Mock<IDonorUseCases> _donorUseCasesMock;
        private Mock<IValidator<CreateDonorCommand>> _validatorMock;
        private CreateDonorCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _donorUseCasesMock = new Mock<IDonorUseCases>();
            _validatorMock = new Mock<IValidator<CreateDonorCommand>>();
            _handler = new CreateDonorCommandHandler(_donorUseCasesMock.Object, _validatorMock.Object);
        }

        [Test]
        public void HandleShouldThrowValidationExceptionWhenValidationFails()
        {
            // Arrange
            var command = new CreateDonorCommand { Email = "invalid-email" };
            var validationFailure = new ValidationFailure("Email", "Invalid email format");
            var validationResult = new ValidationResult(new List<ValidationFailure> { validationFailure });

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(exception.Message, Does.Contain("Invalid email format"));
        }

        [Test]
        public void Handle_ShouldThrowApplicationException_WhenEmailAlreadyExists()
        {
            // Arrange
            var command = new CreateDonorCommand { Email = "johndoe@example.com" };

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            _donorUseCasesMock.Setup(x => x.EmailExistsAsync(command.Email)).ReturnsAsync(true);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ApplicationException>(async () => await _handler.Handle(command, CancellationToken.None));
            Assert.That(exception.Message, Does.Contain("Email already exists"));
        }

        [Test]
        public void Handle_ShouldCreateDonorSuccessfully_WhenValidCommand()
        {
            // Arrange
            var command = new CreateDonorCommand
            {
                FullName = "John Doe",
                Email = "johndoe@example.com",
                BirthDate = DateTime.Parse("1985-05-15"),
                Gender = Domain.Enums.v1.DonorGender.MALE,
                Weight = 75.5,
                BloodType = "O",
                RhFactor = "+",
                Address = new AddressDto
                {
                    Street = "Rua Brasil",
                    Number = "123",
                    Neighborhood = "Centro",
                    City = "Bastos",
                    State = "SP",
                    ZipCode = "17690-000"
                }
            };

            var createdDonor = new DonorDto
            {
                Id = Guid.NewGuid(),
                FullName = "John Doe",
                Email = "johndoe@example.com",
                BirthDate = DateTime.Parse("1985-05-15"),
                Gender = Domain.Enums.v1.DonorGender.MALE,
                Weight = 75.5,
                BloodType = "O",
                RhFactor = "+",
                Address = command.Address
            };

            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());
            _donorUseCasesMock.Setup(x => x.EmailExistsAsync(command.Email)).ReturnsAsync(false);
            _donorUseCasesMock.Setup(x => x.CreateDonorAsync(It.IsAny<DonorDto>())).ReturnsAsync(createdDonor);

            // Act
            var result = _handler.Handle(command, CancellationToken.None).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(createdDonor.Id));
                Assert.That(result.FullName, Is.EqualTo(createdDonor.FullName));
                Assert.That(result.Email, Is.EqualTo(createdDonor.Email));
                Assert.That(result.BirthDate, Is.EqualTo(createdDonor.BirthDate));
                Assert.That(result.Gender, Is.EqualTo(createdDonor.Gender));
                Assert.That(result.Weight, Is.EqualTo(createdDonor.Weight));
                Assert.That(result.BloodType, Is.EqualTo(createdDonor.BloodType));
                Assert.That(result.RhFactor, Is.EqualTo(createdDonor.RhFactor));
                Assert.That(result.Address, Is.EqualTo(createdDonor.Address));
            });
        }
    }
}
