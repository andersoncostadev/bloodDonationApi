namespace Application.Dtos
{
    public class DonationReportDto
    {
        public Guid DonationId { get; set; }
        public Guid DonorId { get; set; }
        public string? DonorFullName { get; set; }
        public DateTime? DonationDate { get; set; }
        public int? QuantityML { get; set; } 
        public string? BloodType { get; set; } 
        public string? RhFactor { get; set; } 
    }
}
