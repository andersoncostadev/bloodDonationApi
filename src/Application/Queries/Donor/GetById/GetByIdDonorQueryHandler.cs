using Application.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Queries.Donor.GetById
{
    public class GetByIdDonorQueryHandler : IRequestHandler<GetByIdDonorQuery, GetByIdDonorQueryResponse>
    {
        private readonly IDonorUseCases _donorUseCases;
        private readonly IValidator<GetByIdDonorQuery> _validator;

        public GetByIdDonorQueryHandler(IDonorUseCases donorUseCases, IValidator<GetByIdDonorQuery> validator)
        {
            _donorUseCases = donorUseCases;
            _validator = validator;
        }

        public async Task<GetByIdDonorQueryResponse> Handle(GetByIdDonorQuery request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var donor = await _donorUseCases.GetDonorByIdAsync(request.Id);

            return donor == null
                ? throw new ApplicationException("Donor not found")
                : new GetByIdDonorQueryResponse(
                    donor.Id,
                    donor.FullName,
                    donor.Email,
                    donor.BirthDate,
                    donor.Gender,
                    donor.Weight,
                    donor.BloodType,
                    donor.RhFactor,
                    donor.Address,
                    donor.Donations);
        }
    }
}
