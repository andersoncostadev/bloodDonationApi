using MediatR;

namespace Application.Commands.v1.Donor.Delete
{
    public class DeleteDonorCommand : IRequest<DeleteDonorCommandResponse>
    {
        public DeleteDonorCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
