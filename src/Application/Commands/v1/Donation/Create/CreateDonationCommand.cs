using MediatR;

namespace Application.Commands.v1.Donation.Create
{
    public class CreateDonationCommand : IRequest<CreateDonationCommandResponse>
    {
        public Guid DonorId { get; set; }
        public DateTime? DonationDate { get; set; }
        public int? QuantityML { get; set; }
    }
}
