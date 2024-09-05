using Application.Dtos;
using Domain.Enums.v1;

namespace Application.Queries.Donor.GetById
{
    public class GetByIdDonorQueryResponse
    {
        public GetByIdDonorQueryResponse(Guid id, string? fullName, string? email, DateTime? birthDate, DonorGender? gender, double? weight, string? bloodType, string? rhFactor, AddressDto? address, List<DonationDto>? donations)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            BirthDate = birthDate;
            Gender = gender;
            Weight = weight;
            BloodType = bloodType;
            RhFactor = rhFactor;
            Address = address;
            Donations = donations;
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
