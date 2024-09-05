using FluentValidation;

namespace Application.Queries.Donor.GetById
{
    public class GetByIdDonorValidator : AbstractValidator<GetByIdDonorQuery>
    {
        public GetByIdDonorValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
