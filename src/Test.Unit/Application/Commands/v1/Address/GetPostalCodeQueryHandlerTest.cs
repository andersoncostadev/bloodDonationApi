using Application.Dtos;
using Application.Interfaces;
using Application.Queries.Address.GetPostalCode;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography;

namespace Test.Unit.Application.Commands.v1.Address
{
    public class GetPostalCodeQueryHandlerTest
    {
        private Mock<IAddressUseCases> _addressUseCases;
        private Mock<IValidator<GetPostalCodeQuery>> _validator;
        private GetPostalCodeQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _addressUseCases = new Mock<IAddressUseCases>();
            _validator = new Mock<IValidator<GetPostalCodeQuery>>();
            _handler = new GetPostalCodeQueryHandler(_addressUseCases.Object, _validator.Object);
        }

        [Test]
        public async Task GetAddressByPostalCodeAsync_ShouldReturnCorrectAddress_WhenPostalCodeIsValid()
        {
            // Arrange
            var postalCode = "12345-678";
            var viaCepResponse = new ViaCepResponse
            {
                Logradouro = "Rua Brasil",
                Bairro = "Jardim América",
                Localidade = "São Paulo",
                Uf = "SP",
                Cep = postalCode
            };

            var responseContent = JsonConvert.SerializeObject(viaCepResponse);

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object);

            var postalCodeService = new PostalCodeService(httpClient);

            // Act
            var address = await postalCodeService.GetAddressByPostalCodeAsync(postalCode);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(address.Street, Is.EqualTo("Rua Brasil"));
                Assert.That(address.Neighborhood, Is.EqualTo("Jardim América"));
                Assert.That(address.City, Is.EqualTo("São Paulo"));
                Assert.That(address.State, Is.EqualTo("SP"));
                Assert.That(address.ZipCode, Is.EqualTo(postalCode));
            });
        }

        [Test]
        public void GetAddressByPostalCodeAsync_ShouldReturnNull_WhenPostalCodeIsInvalid()
        {
            // Arrange
            var postalCode = "12345-678";
            var responseContent = JsonConvert.SerializeObject(new ViaCepResponse());

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent(responseContent)
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object);

            var postalCodeService = new PostalCodeService(httpClient);

            // Act
            var address = postalCodeService.GetAddressByPostalCodeAsync(postalCode).Result;

            // Assert
            Assert.That(address, Is.Null);
        }

        [Test]
        public void GetAddressByPostalCodeAsync_ShouldReturnNull_WhenApiResponseIsInvalid()
        {
            //Arrange
            var postalCode = "12345-678";

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{ invalidJson }")
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            var postalCodeService = new PostalCodeService(httpClient);

            //Act
            var address = postalCodeService.GetAddressByPostalCodeAsync(postalCode).Result;

            //Assert
            Assert.That(address, Is.Null);
        }

        [Test]
        public void GetAddressByPostalCodeAsync_ShouldThrowException_WhenHttpClientFails()
        {
            // Arrange
            var postalCode = "12345-678";

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network failure"));

            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            var postalCodeService = new PostalCodeService(httpClient);

            // Act & Assert
            Assert.ThrowsAsync<HttpRequestException>(() => postalCodeService.GetAddressByPostalCodeAsync(postalCode));
        }

        [Test]
        public void GetAddressByPostalCodeAsync_ShouldReturnNull_WhenApiResponseIsEmpty()
        {
            // Arrange
            var postalCode = "12345-678";

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            var postalCodeService = new PostalCodeService(httpClient);

            // Act
            var address = postalCodeService.GetAddressByPostalCodeAsync(postalCode).Result;

            // Assert
            Assert.That(address, Is.Null);
        }

        [Test]
        public void GetAddressByPostalCodeAsync_ShouldReturnFullAddress_WhenApiResponseIsValid()
        {
            // Arrange
            var postalCode = "12345-678";

            var viaCepResponse = new ViaCepResponse
            {
                Logradouro = "Rua Brasil",
                Bairro = "Jardim América",
                Localidade = "São Paulo",
                Uf = "SP",
                Cep = postalCode
            };

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(viaCepResponse))
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            var postalCodeService = new PostalCodeService(httpClient);

            // Act
            var address = postalCodeService.GetAddressByPostalCodeAsync(postalCode).Result;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(address.Street, Is.EqualTo("Rua Brasil"));
                Assert.That(address.Neighborhood, Is.EqualTo("Jardim América"));
                Assert.That(address.City, Is.EqualTo("São Paulo"));
                Assert.That(address.State, Is.EqualTo("SP"));
                Assert.That(address.ZipCode, Is.EqualTo(postalCode));
            });
        }

        [Test]
        public void Handle_ShouldReturnPostalCodeResponse_WhenPostalCodeIsValid()
        {
            // Arrange
            var query = new GetPostalCodeQuery("12345-678");

            var address = new AddressDto
            {
                Street = "Rua Brasil",
                Neighborhood = "Jardim América",
                City = "São Paulo",
                State = "SP",
                ZipCode = "12345-678"
            };

            var validationResult = new ValidationResult();

            _validator.Setup(x => x.ValidateAsync(query, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());


            _addressUseCases.Setup(x => x.GetByPostalCodeAsync(query.PostalCode)).ReturnsAsync(address);

            // Act
            var response = _handler.Handle(query, CancellationToken.None).Result;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Street, Is.EqualTo("Jardim América"));
                Assert.That(response.Neighborhood, Is.EqualTo("Rua Brasil"));
                Assert.That(response.City, Is.EqualTo("São Paulo"));
                Assert.That(response.State, Is.EqualTo("SP"));
                Assert.That(response.ZipCode, Is.EqualTo("12345-678"));
            });
        }

        [Test]
        public void Handle_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var query = new GetPostalCodeQuery("12345-678");
            var validationFailure = new ValidationFailure("PostalCode", "Invalid PostalCode");
            var validationResult = new ValidationResult(new[] { validationFailure });

            var validatorMock = new Mock<IValidator<GetPostalCodeQuery>>();
            validatorMock.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var addressUseCasesMock = new Mock<IAddressUseCases>();

            var handler = new GetPostalCodeQueryHandler(addressUseCasesMock.Object, validatorMock.Object);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await handler.Handle(query, CancellationToken.None));

            Assert.That(exception.Message, Does.Contain("Invalid PostalCode"));
        }

        [Test]
        public void Handle_ShouldThrowApplicationException_WhenAddressNotFound()
        {
            // Arrange
            var query = new GetPostalCodeQuery("12345-678");

            // Simula uma validação bem-sucedida
            var validationMock = new Mock<IValidator<GetPostalCodeQuery>>();
            validationMock.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            // Simula a ausência de endereço retornado pelo caso de uso
            _addressUseCases.Setup(x => x.GetByPostalCodeAsync(query.PostalCode)).ReturnsAsync((AddressDto)null);

            _handler = new GetPostalCodeQueryHandler(_addressUseCases.Object, validationMock.Object);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ApplicationException>(async () => await _handler.Handle(query, CancellationToken.None));
            Assert.That(exception.Message, Does.Contain("Address not found"));
        }

    }
}
