using Application.Dtos;
using Application.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Commands.v1.Donor.Create
{
    public class CreateDonorCommandHandler : IRequestHandler<CreateDonorCommand, CreateDonorCommandResponse>
    {
        private readonly IDonorUseCases _donorUseCases;
        private readonly IValidator<CreateDonorCommand> _validator;

        public CreateDonorCommandHandler(IDonorUseCases donorUseCases, IValidator<CreateDonorCommand> validator)
        {
            _donorUseCases = donorUseCases;
            _validator = validator;
        }

        public async Task<CreateDonorCommandResponse> Handle(CreateDonorCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            if (await _donorUseCases.EmailExistsAsync(request.Email!))
                throw new ApplicationException("Email already exists");

            var address = new AddressDto
            {
                Street = request.Address!.Street,
                Number = request.Address.Number,
                Neighborhood = request.Address.Neighborhood,
                City = request.Address.City,
                State = request.Address.State,
                ZipCode = request.Address.ZipCode
            };

            var donor = new DonorDto
            {
                FullName = request.FullName,
                Email = request.Email,
                BirthDate = request.BirthDate,
                Gender = request.Gender,
                Weight = request.Weight,
                BloodType = request.BloodType,
                RhFactor = request.RhFactor,
                Address = address
            };

            var createdDonor = await _donorUseCases.CreateDonorAsync(donor);

            return createdDonor == null
                ? throw new ApplicationException("Failed to create donor")
                : new CreateDonorCommandResponse(
                    createdDonor.Id,
                    createdDonor.FullName,
                    createdDonor.Email,
                    createdDonor.BirthDate,
                    createdDonor.Gender,
                    createdDonor.Weight,
                    createdDonor.BloodType,
                    createdDonor.RhFactor,
                    createdDonor.Address);
        }

    }
}
