using AutoMapper;
using BusinessObject.Model;
using DTO.CategoryDTO;

namespace Repository.MapperConfig
{
    public partial class AutoMapperConfig : Profile
    {
        void CategoryMapper()
        {
            //CreateMap
            CreateMap<CategoryUpsert,Category>().ReverseMap();
        }
    }
}
