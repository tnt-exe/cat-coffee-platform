using AutoMapper;
using BusinessObject.Model;
using DTO.AreaDTO;

namespace Repository.MapperConfig
{
    public partial class AutoMapperConfig : Profile
    {
        void AreaMapper()
        {
            //CreateMap
            CreateMap<Area, AreaDto>()
                .ForMember(dest => dest.CoffeeShop,
                            opt => opt.MapFrom(src => src.CoffeeShop!.ShopName))
                .ReverseMap();
            CreateMap<Area, AreaCreate>().ReverseMap();
            CreateMap<Area, AreaUpdate>().ReverseMap();
        }
    }
}
