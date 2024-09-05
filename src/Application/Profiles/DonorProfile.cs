using Application.Dtos;
using AutoMapper;
using Domain.Entities.v1;

namespace Application.Profiles
{
    public class DonorProfile : Profile
    {
        public DonorProfile()
        {
            CreateMap<DonorDto, DonorEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Donations, opt => opt.MapFrom(src => src.Donations));


            CreateMap<DonorEntity, DonorDto>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Donations, opt => opt.MapFrom(src => src.Donations));

        }
    }
}
