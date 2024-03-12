using AutoMapper;
using BusinessObject.Model;
using DTO;

namespace Repository.MapperConfig
{
    public partial class AutoMapperConfig : Profile
    {
        void CategoryMapper()
        {
            //CreateMap
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryCreate>().ReverseMap();
            CreateMap<Category, CategoryUpdate>().ReverseMap();
        }
    }
}
