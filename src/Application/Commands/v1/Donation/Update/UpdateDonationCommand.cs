using MediatR;

namespace Application.Commands.v1.Donation.Update
{
    public class UpdateDonationCommand : IRequest<UpdateDonationCommandResponse>
    {
        public Guid Id { get; set; }
        public Guid DonorId { get; set; }
        public DateTime? DonationDate { get; set; }
        public int? QuantityML { get; set; }
    }
}
