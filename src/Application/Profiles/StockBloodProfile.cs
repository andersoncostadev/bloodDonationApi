using Application.Dtos;
using AutoMapper;
using Domain.Entities.v1;

namespace Application.Profiles
{
    public class StockBloodProfile : Profile
    {
        public StockBloodProfile()
        {
            CreateMap<StockBloodDto, StockBloodEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<StockBloodEntity, StockBloodDto>();
        }
    }
}
