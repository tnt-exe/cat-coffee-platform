using AutoMapper;
using BusinessObject.Model;
using DTO.CoffeeShopDTO;

namespace Repository.MapperConfig
{
    public partial class AutoMapperConfig : Profile
    {
        void CoffeeShopMapper()
        {
            CreateMap<CoffeeShop, CoffeeShopResponseDTO>();

            CreateMap<CoffeeShop, CoffeeShopResponseDTO>().ForMember(dest => dest.ManagerEmail, opt => opt.MapFrom(src => src.Manager!.Email)).ReverseMap();
        }
    }
}
