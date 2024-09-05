using FluentValidation;

namespace Application.Commands.v1.Donor.Update
{
    public class UpdateDonorValidator : AbstractValidator<UpdateDonorCommand>
    {
        public UpdateDonorValidator()
        {
            RuleFor(x => x.FullName)
               .NotEmpty().WithMessage("FullName is required")
               .MaximumLength(100).WithMessage("FullName must not exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid");

            RuleFor(x => x.Weight)
                .NotEmpty().WithMessage("Weight is required")
                .GreaterThan(0).WithMessage("Weight must be greater than 0");
        }
    }
}
