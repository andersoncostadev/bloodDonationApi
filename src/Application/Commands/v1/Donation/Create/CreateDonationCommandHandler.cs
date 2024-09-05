using Application.Dtos;
using Application.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Commands.v1.Donation.Create
{
    public class CreateDonationCommandHandler : IRequestHandler<CreateDonationCommand, CreateDonationCommandResponse>
    {
        private readonly IDonationUseCases _donationUseCases;
        private readonly IDonorUseCases _donorUseCases;
        private readonly IStockBloodUseCases _stockBloodUseCases;
        private readonly IValidator<CreateDonationCommand> _validator;

        public CreateDonationCommandHandler(IDonationUseCases donationUseCases, IDonorUseCases donorUseCases, IStockBloodUseCases stockBloodUseCases ,IValidator<CreateDonationCommand> validator)
        {
            _donationUseCases = donationUseCases;
            _donorUseCases = donorUseCases;
            _stockBloodUseCases = stockBloodUseCases;
            _validator = validator;
        }

        public async Task<CreateDonationCommandResponse> Handle(CreateDonationCommand request, CancellationToken cancellationToken)
        {
           var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var donor = await _donorUseCases.GetDonorByIdAsync(request.DonorId)
                ?? throw new ApplicationException("Donor not found");

            var donation = new DonationDto
            {
                DonorId = donor.Id,
                DonationDate = request.DonationDate,
                QuantityML = request.QuantityML
            };

            var createdDonation = await _donationUseCases.CreateDonationAsync(donation) ?? throw new ApplicationException("Failed to create donation");

            var stockBlood = new StockBloodDto
            {
                BloodType = donor.BloodType,
                RhFactor = donor.RhFactor,
                QuantityML = donation.QuantityML
            };

            await _stockBloodUseCases.UpdateStockBloodAsync(stockBlood);

            return new CreateDonationCommandResponse(
                    createdDonation.Id,
                    createdDonation.DonorId,
                    createdDonation.DonationDate,
                    createdDonation.QuantityML
            );
        }
    }
}
