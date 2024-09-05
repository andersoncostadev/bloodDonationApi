using FluentValidation;

namespace Application.Commands.v1.Donation.Update
{
    public class UpdateDonationValidator : AbstractValidator<UpdateDonationCommand>
    {
        public UpdateDonationValidator()
        {
            RuleFor(x => x.DonorId)
                .NotEmpty()
                .WithMessage("DonorId is required");

            RuleFor(x => x.QuantityML)
               .NotEmpty()
               .WithMessage("Quantity ML cannot be empty")
               .InclusiveBetween(420, 470)
               .WithMessage("Quantity ML must be between 420 and 470");
        }
    }
}
