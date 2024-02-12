using AutoMapper;
using BusinessObject.Model;
using DTO.TimeFrameDTO;

namespace Repository.MapperConfig
{
    public partial class AutoMapperConfig : Profile
    {
        void TimeFrameMapper()
        {
            //CreateMap
            CreateMap<TimeFrame, TimeFrameDto>()
                .ForMember(dest => dest.CoffeeShop,
                            opt => opt.MapFrom(src => src.CoffeeShop!.ShopName))
                .ReverseMap();
            CreateMap<TimeFrame, TimeFrameCreate>().ReverseMap();
            CreateMap<TimeFrame, TimeFrameUpdate>().ReverseMap();
        }
    }
}
