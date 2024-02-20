using AutoMapper;
using BusinessObject.Model;
using DTO.CoffeeShopDTO;

namespace Repository.MapperConfig
{
    public partial class AutoMapperConfig : Profile
    {
        void CoffeeShopMapper()
        {
            CreateMap<CoffeeShop, CoffeeShopResponseDTO>()
                .ForMember(dest => dest.OpeningTime,
                            opt => opt.MapFrom(src => src.OpeningTime.ToString("HH:mm")))
                .ForMember(dest => dest.ClosingTime,
                            opt => opt.MapFrom(src => src.ClosingTime.ToString("HH:mm")))
                .ForMember(dest => dest.ManagerEmail, 
                            opt => opt.MapFrom(src => src.Manager!.Email))
                .ReverseMap();
        }
    }
}
