using Application.Dtos;
using AutoMapper;
using Domain.Entities.v1;

namespace Application.Profiles
{
    public class DonationProfile : Profile
    {
        public DonationProfile()
        {
            CreateMap<DonationDto, DonationEntity>();

            CreateMap<DonationEntity, DonationDto>();

            CreateMap<DonationDto, DonationReportDto>()
                .ForMember(dest => dest.DonationId, opt => opt.MapFrom(src => src.Id))  
                .ForMember(dest => dest.DonorId, opt => opt.MapFrom(src => src.DonorId)) 
                .ForMember(dest => dest.BloodType, opt => opt.Ignore())
                .ForMember(dest => dest.RhFactor, opt => opt.Ignore())
                .ForMember(dest => dest.DonationDate, opt => opt.MapFrom(src => src.DonationDate))
                .ForMember(dest => dest.QuantityML, opt => opt.MapFrom(src => src.QuantityML)); ;
        }
    }
}
