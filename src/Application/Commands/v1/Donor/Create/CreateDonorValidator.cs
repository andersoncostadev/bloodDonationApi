using FluentValidation;

namespace Application.Commands.v1.Donor.Create
{
    public class CreateDonorValidator : AbstractValidator<CreateDonorCommand>
    {
        public CreateDonorValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName is required")
                .MaximumLength(100).WithMessage("FullName must not exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("BirthDate is required")
                .LessThan(DateTime.Now).WithMessage("BirthDate must be less than current date");

            RuleFor(x => x.Gender)
                .NotNull().WithMessage("Gender is required");

            RuleFor(x => x.Weight)
                .NotEmpty().WithMessage("Weight is required")
                .GreaterThan(0).WithMessage("Weight must be greater than 0");

            RuleFor(x => x.BloodType)
                .NotEmpty().WithMessage("BloodType is required")
                .MaximumLength(3).WithMessage("BloodType must not exceed 3 characters");

            RuleFor(x => x.RhFactor)
                .NotEmpty().WithMessage("RhFactor is required")
                .MaximumLength(1).WithMessage("RhFactor must not exceed 1 character");

            RuleFor(x => x.Address!.Street)
               .NotEmpty().WithMessage("Street is required")
               .MaximumLength(150).WithMessage("Street must not exceed 150 characters");

            RuleFor(x => x.Address!.Number)
                .NotEmpty().WithMessage("Number is required")
                .MaximumLength(10).WithMessage("Number must not exceed 10 characters");

            RuleFor(x => x.Address!.Neighborhood)
                .NotEmpty().WithMessage("Neighborhood is required")
                .MaximumLength(100).WithMessage("Neighborhood must not exceed 100 characters");

            RuleFor(x => x.Address!.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(100).WithMessage("City must not exceed 100 characters");

            RuleFor(x => x.Address!.State)
                .NotEmpty().WithMessage("State is required")
                .MaximumLength(50).WithMessage("State must not exceed 50 characters");

            RuleFor(x => x.Address!.ZipCode)
                .NotEmpty().WithMessage("ZipCode is required")
                .Matches(@"^\d{5}-\d{3}$").WithMessage("ZipCode must be in the format 00000-000");

        }
    }
}
