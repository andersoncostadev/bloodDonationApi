using Domain.Enums.v1;

namespace Domain.Entities.v1
{
    public class DonorEntity : BaseEntity
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public DonorGender Gender { get; set; }
        public double? Weight { get; set; }
        public string? BloodType { get; set; }
        public string? RhFactor { get; set; }
        public List<DonationEntity>? Donations { get; set; } = [];
        public  AddressEntity? Address { get; set; }
    }
}
