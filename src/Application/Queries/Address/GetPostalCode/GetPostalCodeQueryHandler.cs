using Application.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Queries.Address.GetPostalCode
{
    public class GetPostalCodeQueryHandler : IRequestHandler<GetPostalCodeQuery, GetPostalCodeQueryResponse>
    {
        private readonly IAddressUseCases addressUseCases;
        private readonly IValidator<GetPostalCodeQuery> validator;

        public GetPostalCodeQueryHandler(IAddressUseCases addressUseCases, IValidator<GetPostalCodeQuery> validator)
        {
            this.addressUseCases = addressUseCases;
            this.validator = validator;
        }

        public async Task<GetPostalCodeQueryResponse> Handle(GetPostalCodeQuery request, CancellationToken cancellationToken)
        {
           var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var address = await addressUseCases.GetByPostalCodeAsync(request.PostalCode);

            return address == null
                ? throw new ApplicationException("Address not found")
                : new GetPostalCodeQueryResponse(
                    address.Neighborhood,
                    address.Street,
                    address.City,
                    address.State,
                    address.ZipCode
                );
        }
    }
}
