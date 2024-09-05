using Domain.Enums.v1;

namespace Application.Dtos
{
    public class DonorDto
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public DonorGender Gender { get; set; }
        public double? Weight { get; set; }
        public string? BloodType { get; set; }
        public string? RhFactor { get; set; }
        public List<DonationDto>? Donations { get; set; } = [];
        public  AddressDto? Address { get; set; }
    }
}
