using Application.Dtos;
using Domain.Enums.v1;

namespace Application.Queries.Donor.GetFullName
{
    public class GetByFullNameQueryResponse 
    {
        public GetByFullNameQueryResponse(Guid id, string? fullName, string? email, DateTime? birthDate, DonorGender? gender, double? weight, string? bloodType, string? rhFactor, List<DonationDto>? donations, AddressDto? address)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            BirthDate = birthDate;
            Gender = gender;
            Weight = weight;
            BloodType = bloodType;
            RhFactor = rhFactor;
            Donations = donations;
            Address = address;
        }

        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public DonorGender? Gender { get; set; }
        public double? Weight { get; set; }
        public string? BloodType { get; set; }
        public string? RhFactor { get; set; }
        public List<DonationDto>? Donations { get; set; } = [];
        public AddressDto? Address { get; set; }
    }
}
