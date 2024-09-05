namespace Application.Dtos
{
    public class DonationDto
    {
        public Guid Id { get; set; }
        public Guid DonorId { get; set; }
        public DateTime? DonationDate { get; set; }
        public int? QuantityML { get; set; }
    }
}
