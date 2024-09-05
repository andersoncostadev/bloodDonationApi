using Application.Dtos;
using Domain.Enums.v1;
using MediatR;

namespace Application.Commands.v1.Donor.Create
{
    public class CreateDonorCommand : IRequest<CreateDonorCommandResponse>
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public DonorGender Gender { get; set; }
        public double? Weight { get; set; }
        public string? BloodType { get; set; }
        public string? RhFactor { get; set; }
        public AddressDto? Address { get; set; }

    }
}
