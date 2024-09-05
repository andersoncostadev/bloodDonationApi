using Application.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Queries.Donor.GetFullName
{
    public class GetByFullNameQueryHandler : IRequestHandler<GetByFullNameQuery, GetByFullNameQueryResponse>
    {
        private readonly IDonorUseCases _donorUseCases;
        private readonly IValidator<GetByFullNameQuery> _validator;

        public GetByFullNameQueryHandler(IDonorUseCases donorUseCases, IValidator<GetByFullNameQuery> validator)
        {
            _donorUseCases = donorUseCases;
            _validator = validator;
        }

        public async Task<GetByFullNameQueryResponse> Handle(GetByFullNameQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var donor = await _donorUseCases.GetDonorFullNameAsync(request.FullName!);

            return donor == null
                ? throw new ApplicationException("Donor not found")
                : new GetByFullNameQueryResponse(
                     donor.Id,
                    donor.FullName,
                    donor.Email,
                    donor.BirthDate,
                    donor.Gender,
                    donor.Weight,
                    donor.BloodType,
                    donor.RhFactor,
                    donor.Donations,
                    donor.Address);

        }
    }
}
