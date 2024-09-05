using FluentValidation;

namespace Application.Commands.v1.Donor.Delete
{
    public class DeleteDonorValidation : AbstractValidator<DeleteDonorCommand>
    {
        public DeleteDonorValidation()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
