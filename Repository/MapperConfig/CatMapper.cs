using AutoMapper;
using BusinessObject.Model;
using DTO.CatDTO;

namespace Repository.MapperConfig
{
    public partial class AutoMapperConfig : Profile
    {
        void CatMapper()
        {
            //CreateMap
            CreateMap<Cat, CatDto>()
                .ForMember(dest => dest.Area,
                            opt => opt.MapFrom(src => src.Area!.AreaName))
                .ForMember(dest => dest.CoffeeShop,
                            opt => opt.MapFrom(src => src.CoffeeShop!.ShopName))
                .ReverseMap();
            CreateMap<Cat, CatCreate>().ReverseMap();
            CreateMap<Cat, CatUpdate>().ReverseMap();
        }
    }
}
