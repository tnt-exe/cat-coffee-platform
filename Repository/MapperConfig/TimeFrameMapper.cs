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
                .ForMember(dest => dest.StartTime,
                            opt => opt.MapFrom(src => src.StartTime.ToString("HH:mm")))
                .ForMember(dest => dest.EndTime,
                            opt => opt.MapFrom(src => src.EndTime.ToString("HH:mm")))
                .ReverseMap();

            CreateMap<TimeFrameCreate, TimeFrame>()
                .ForMember(dest => dest.StartTime,
                            opt => opt.MapFrom(src => TimeOnly.Parse(src.StartTime!)))
                .ForMember(dest => dest.EndTime,
                            opt => opt.MapFrom(src => TimeOnly.Parse(src.EndTime!)))
                .ReverseMap();

            CreateMap<TimeFrameUpdate, TimeFrame>()
                .ForMember(dest => dest.StartTime,
                            opt => opt.MapFrom(src => TimeOnly.Parse(src.StartTime!)))
                .ForMember(dest => dest.EndTime,
                            opt => opt.MapFrom(src => TimeOnly.Parse(src.EndTime!)))
                .ReverseMap();
        }
    }
}
