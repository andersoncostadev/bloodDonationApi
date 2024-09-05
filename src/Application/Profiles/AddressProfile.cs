using Application.Dtos;
using AutoMapper;
using Domain.Entities.v1;

namespace Application.Profiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressDto, AddressEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DonorId, opt => opt.MapFrom(src => src.DonorId));

            CreateMap<AddressEntity, AddressDto>();
        }
    }
}
