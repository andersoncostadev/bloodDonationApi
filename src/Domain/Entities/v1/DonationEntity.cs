namespace Domain.Entities.v1
{
    public class DonationEntity : BaseEntity
    {
        public Guid DonorId { get; set; }
        public DateTime? DonationDate { get; set; }
        public int? QuantityML { get; set; }
        public DonorEntity? Donor { get; set; }
    }
}
