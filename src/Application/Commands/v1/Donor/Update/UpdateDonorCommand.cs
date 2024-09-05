using Application.Dtos;
using MediatR;

namespace Application.Commands.v1.Donor.Update
{
    public class UpdateDonorCommand : IRequest<UpdateDonorCommandResponse>
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public double? Weight { get; set; }
        public AddressDto? Address { get; set; }
    }
}
