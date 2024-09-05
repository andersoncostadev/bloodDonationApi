using FluentValidation;

namespace Application.Queries.Address.GetPostalCode
{
    public class GetPostalCodeValidator : AbstractValidator<GetPostalCodeQuery>
    {
        public GetPostalCodeValidator()
        {
            RuleFor(x => x.PostalCode)
                .NotEmpty()
                .WithMessage("Postal code is required")
                .Length(8)
                .WithMessage("Postal code must have 8 characters");
        }
    }
}
