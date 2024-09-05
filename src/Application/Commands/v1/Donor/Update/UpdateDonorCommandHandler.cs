using Application.Dtos;
using Application.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Commands.v1.Donor.Update
{
    public class UpdateDonorCommandHandler : IRequestHandler<UpdateDonorCommand, UpdateDonorCommandResponse>
    {
        private readonly IDonorUseCases _donorUseCases;
        private readonly IAddressUseCases _addressUseCases;
        private readonly IValidator<UpdateDonorCommand> _validator;

        public UpdateDonorCommandHandler(IDonorUseCases donorUseCases, IAddressUseCases addressUseCases, IValidator<UpdateDonorCommand> validator)
        {
            _donorUseCases = donorUseCases;
            _addressUseCases = addressUseCases;
            _validator = validator;
        }

        public async Task<UpdateDonorCommandResponse> Handle(UpdateDonorCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var donor = await _donorUseCases.GetDonorByIdAsync(request.Id)
                ?? throw new Exception("Donor not found");

            donor.FullName = request.FullName;
            donor.Email = request.Email;
            donor.Weight = request.Weight;

            if(request.Address != null)
            {
                var addressDto = donor.Address ?? new AddressDto();
                addressDto.Street = request.Address.Street;
                addressDto.Number = request.Address.Number;
                addressDto.Neighborhood = request.Address.Neighborhood;
                addressDto.City = request.Address.City;
                addressDto.State = request.Address.State;
                addressDto.ZipCode = request.Address.ZipCode;

                addressDto.DonorId = donor.Id;

                donor.Address = addressDto;
            }


            var updatedDonor = await _donorUseCases.UpdateDonorAsync(donor)
                ?? throw new Exception("Failed to update donor");

            return new UpdateDonorCommandResponse(
                updatedDonor.Id,
                updatedDonor.FullName,
                updatedDonor.Email,
                updatedDonor.BirthDate,
                updatedDonor.Gender,
                updatedDonor.Weight,
                updatedDonor.BloodType,
                updatedDonor.RhFactor,
                updatedDonor.Address);
        }
    }
}
