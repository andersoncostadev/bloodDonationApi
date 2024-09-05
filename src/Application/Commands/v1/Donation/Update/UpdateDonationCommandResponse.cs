namespace Application.Commands.v1.Donation.Update
{
    public class UpdateDonationCommandResponse
    {
        public UpdateDonationCommandResponse(Guid id, Guid donorId, DateTime? donationDate, int? quantityML)
        {
            Id = id;
            DonorId = donorId;
            DonationDate = donationDate;
            QuantityML = quantityML;
        }

        public Guid Id { get; set; }
        public Guid DonorId { get; set; }
        public DateTime? DonationDate { get; set; }
        public int? QuantityML { get; set; }
    }
}
