using Application.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Commands.v1.Donor.Delete
{
    public class DeleteDonorCommandHandler : IRequestHandler<DeleteDonorCommand, DeleteDonorCommandResponse>
    {
        private readonly IDonorUseCases _donorUseCases;
        private readonly IValidator<DeleteDonorCommand> _validator;

        public DeleteDonorCommandHandler(IDonorUseCases donorUseCases, IValidator<DeleteDonorCommand> validator)
        {
            _donorUseCases = donorUseCases;
            _validator = validator;
        }

        public async Task<DeleteDonorCommandResponse> Handle(DeleteDonorCommand request, CancellationToken cancellationToken)
        {

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var donor = await _donorUseCases.GetDonorByIdAsync(request.Id) ?? throw new ApplicationException("Donor not found");
            var deletedDonor = await _donorUseCases.DeleteDonorAsync(donor.Id);

           if(!deletedDonor)
                throw new ApplicationException("Failed to delete donor");

            return new DeleteDonorCommandResponse();
        }
    }
}
