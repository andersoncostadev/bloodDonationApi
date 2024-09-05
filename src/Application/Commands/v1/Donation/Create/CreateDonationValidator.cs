using Application.Interfaces;
using Domain.Enums.v1;
using FluentValidation;

namespace Application.Commands.v1.Donation.Create
{
    public class CreateDonationValidator : AbstractValidator<CreateDonationCommand>
    {
        private readonly IDonorUseCases _donorUseCases;
        public CreateDonationValidator(IDonorUseCases donorUseCases)
        {
            _donorUseCases = donorUseCases;

            RuleFor(x => x.DonationDate)
                .NotEmpty()
                .WithMessage("Donation Date cannot be empty");

            RuleFor(x => x.QuantityML)
                .NotEmpty()
                .WithMessage("Quantity ML cannot be empty")
                .InclusiveBetween(420, 470)
                .WithMessage("Quantity ML must be between 420 and 470");

            RuleFor(x => x)
                .MustAsync(BeEligibleToDonate)
                .WithMessage("Donor is not eligible to donate");
        }

        private async Task<bool> BeEligibleToDonate(CreateDonationCommand command, CancellationToken cancellationToken)
        {
            var donor = await _donorUseCases.GetDonorByIdAsync(command.DonorId);

            if (donor == null)
                return false;

            var age = DateTime.Now.Year - donor.BirthDate!.Value.Year;
            if(donor.BirthDate!.Value.Date > DateTime.Now.AddYears(-age))
                age--;

            if(age < 18)
                return false;

            if (donor.Weight < 50)
                return false;

            var lastDonation = donor.Donations?.OrderByDescending(x => x.DonationDate).FirstOrDefault();

            if (lastDonation != null)
            {
                var daysSinceLastDonation = (command.DonationDate!.Value.Date - lastDonation.DonationDate!.Value.Date).TotalDays;

                if (donor.Gender == DonorGender.FEMALE)
                    return daysSinceLastDonation >= 90;

                if (donor.Gender == DonorGender.MALE)
                    return daysSinceLastDonation >= 60;
            }
            return true;
        }
    }
}
