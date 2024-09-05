using Application.Dtos;
using Application.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Commands.v1.Donation.Update
{
    public class UpdateDonationCommandHandler : IRequestHandler<UpdateDonationCommand, UpdateDonationCommandResponse>
    {
        private readonly IDonationUseCases _donationUseCases;
        private readonly IValidator<UpdateDonationCommand> _validator;

        public UpdateDonationCommandHandler(IDonationUseCases donationUseCases, IValidator<UpdateDonationCommand> validator)
        {
            _donationUseCases = donationUseCases;
            _validator = validator;
        }

        public async Task<UpdateDonationCommandResponse> Handle(UpdateDonationCommand request, CancellationToken cancellationToken)
        {
           var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingDonation = await _donationUseCases.GetDonationByIdAsync(request.Id)
                ?? throw new ApplicationException("Donation not found");

            var donation = new DonationDto
            {
                Id = request.Id,
                DonorId = request.DonorId,
                DonationDate = existingDonation.DonationDate,
                QuantityML = request.QuantityML
            };

            var updatedDonation = await _donationUseCases.UpdateDonationAsync(donation);

            return updatedDonation == null
                ? throw new ApplicationException("Failed to update donation")
                : new UpdateDonationCommandResponse(
                    updatedDonation.Id,
                    updatedDonation.DonorId,
                    updatedDonation.DonationDate,
                    updatedDonation.QuantityML
                );
        }
    }
}
