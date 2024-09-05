using FluentValidation;

namespace Application.Queries.Donor.GetFullName
{
    public class GetByFullNameValidator : AbstractValidator<GetByFullNameQuery>
    {
        public GetByFullNameValidator()
        {
            RuleFor(x => x.FullName).NotEmpty();
        }
    }
}
