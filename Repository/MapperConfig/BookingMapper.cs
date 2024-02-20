using AutoMapper;
using BusinessObject.Model;
using DTO.BookingDTO;

namespace Repository.MapperConfig
{
    public partial class AutoMapperConfig : Profile
    {
        void BookingMapper()
        {
            CreateMap<BookingDTO, Booking>();
            CreateMap<BookingDTO_BookingProduct, BookingProduct>();

            CreateMap<Booking, BookingResponseDTO>()
                .ForMember(dest => dest.CoffeeShop, opt => opt.MapFrom(src => src.CoffeeShop))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area))
                .ForMember(dest => dest.TimeFrame, opt => opt.MapFrom(src => src.TimeFrame));
            CreateMap<CoffeeShop, BookingResponseDTO_CoffeeShop>();
            CreateMap<Area, BookingResponseDTO_Area>();
            CreateMap<TimeFrame, BookingResponseDTO_TimeFrame>();
            CreateMap<User, BookingResponseDTO_User>();
        }
    }
}
