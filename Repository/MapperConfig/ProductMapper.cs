using AutoMapper;
using BusinessObject.Model;
using DTO.ProductDTO;
using Microsoft.Data.SqlClient;

namespace Repository.MapperConfig
{
    public partial class AutoMapperConfig : Profile
    {
        void ProductMapper()
        {
            CreateMap<ProductCreate, Product>().ReverseMap();
            CreateMap<ProductUpdate, Product>()
                .ForMember(dest => dest.ProductName, opt => opt.Condition(src => !String.IsNullOrEmpty(src.ProductName)))
                .ForMember(dest => dest.Price, opt => opt.Condition(src => src.Price != null || src.Price != 0))
                .ForMember(dest => dest.Quantity, opt => opt.Condition(src => src.Quantity != null || src.Quantity != 0))
                .ForMember(dest => dest.Unit, opt => opt.Condition(src => !String.IsNullOrEmpty(src.Unit)))
                .ForMember(dest => dest.CategoryId, opt => opt.Condition(src => src.CategoryId != null || src.CategoryId != 0))
                .ReverseMap();
        }
    }
}
