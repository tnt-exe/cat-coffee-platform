using AutoMapper;
using BusinessObject.Model;
using DTO.UserDTO;

namespace Repository.MapperConfig
{
    public partial class AutoMapperConfig : Profile
    {
        void UserMapper()
        {
            //CreateMap
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName ?? "Undefined"))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName ?? "Undefined"));
            CreateMap<User, UserResponseDTO>();
        }
    }
}
